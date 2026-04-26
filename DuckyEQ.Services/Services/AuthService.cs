using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DuckyEQ.Contracts.DTOs;
using DuckyEQ.Contracts.Interfaces.Repositories;
using DuckyEQ.Contracts.Interfaces.Services;
using DuckyEQ.Contracts.Models;
using DuckyEQ.Domain.Entities;
using DuckyEQ.Domain.Enums;
using DuckyEQ.Services.Exceptions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DuckyEQ.Services.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IUserInventoryRepository _inventoryRepo;
    private readonly IUsernameGenerator _usernameGenerator;
    private readonly IMemoryCache _cache;
    private readonly IConfiguration _config;

    public AuthService(
        IUserRepository userRepo,
        IUserInventoryRepository inventoryRepo,
        IUsernameGenerator usernameGenerator,
        IMemoryCache cache,
        IConfiguration config)
    {
        _userRepo = userRepo;
        _inventoryRepo = inventoryRepo;
        _usernameGenerator = usernameGenerator;
        _cache = cache;
        _config = config;
    }

    public async Task<AuthResult> RegisterAsync(RegisterRequest request)
    {
        var existing = await _userRepo.GetByEmailAsync(request.Email);
        if (existing is not null)
            throw new EmailAlreadyExistsException();

        var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var username = await _usernameGenerator.GenerateUniqueAsync();
        var user = User.Create(request.Email, hash, username, request.KnownAs, request.DuckCharacter);
        await _userRepo.CreateAsync(user);

        // Store email verification token in cache (ACS email sending wired up Day 9)
        var verifyToken = Guid.NewGuid().ToString();
        _cache.Set($"verify:{verifyToken}", user.Id, TimeSpan.FromHours(24));

        return new AuthResult(BuildJwt(user), user.Username, user.KnownAs, user.Id);
    }

    public async Task<AuthResult> LoginAsync(LoginRequest request)
    {
        var user = await _userRepo.GetByEmailAsync(request.Email);
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new InvalidCredentialsException();

        return new AuthResult(BuildJwt(user), user.Username, user.KnownAs, user.Id);
    }

    public async Task<bool> VerifyEmailAsync(string token)
    {
        if (!_cache.TryGetValue($"verify:{token}", out Guid userId))
            return false;

        var user = await _userRepo.GetByIdAsync(userId);
        if (user is null) return false;

        user.VerifyEmail();
        await _userRepo.UpdateAsync(user);
        _cache.Remove($"verify:{token}");
        return true;
    }

    public async Task<UserProfileDto> GetProfileAsync(Guid userId)
    {
        var user = await _userRepo.GetByIdAsync(userId)
            ?? throw new NotFoundException("User not found.");
        var equippedItems = await _inventoryRepo.GetEquippedItemsAsync(userId);
        return new UserProfileDto(
            user.Username,
            user.KnownAs,
            user.DuckCharacter,
            user.OverallXP,
            user.OverallLevel,
            user.StreakDays,
            user.EmailVerified,
            equippedItems);
    }

    public async Task UpdateDuckCharacterAsync(Guid userId, DuckCharacter character)
    {
        await _userRepo.UpdateDuckCharacterAsync(userId, character);
    }

    public async Task<AuthResult> UpdateKnownAsAsync(Guid userId, string knownAs)
    {
        await _userRepo.UpdateKnownAsAsync(userId, knownAs);
        var user = await _userRepo.GetByIdAsync(userId)
            ?? throw new NotFoundException("User not found.");
        return new AuthResult(BuildJwt(user), user.Username, user.KnownAs, user.Id);
    }

    private string BuildJwt(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("username", user.Username),
            new Claim("knownAs", user.KnownAs)
        };
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
