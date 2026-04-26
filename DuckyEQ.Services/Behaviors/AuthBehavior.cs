using DuckyEQ.Contracts.DTOs;
using DuckyEQ.Contracts.Interfaces.Services;
using DuckyEQ.Contracts.Models;
using DuckyEQ.Domain.Enums;

namespace DuckyEQ.Services.Behaviors;

public class AuthBehavior
{
    private readonly IAuthService _authService;
    private readonly ICoinService _coinService;

    public AuthBehavior(IAuthService authService, ICoinService coinService)
    {
        _authService = authService;
        _coinService = coinService;
    }

    public async Task<AuthResult> RegisterAsync(RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);
        await _coinService.AwardAsync(result.UserId, 50); // 50 starter coins
        return result;
    }

    public Task<AuthResult> LoginAsync(LoginRequest request) =>
        _authService.LoginAsync(request);

    public Task<bool> VerifyEmailAsync(string token) =>
        _authService.VerifyEmailAsync(token);

    public Task<UserProfileDto> GetProfileAsync(Guid userId) =>
        _authService.GetProfileAsync(userId);

    public Task<AuthResult> UpdateKnownAsAsync(Guid userId, string knownAs) =>
        _authService.UpdateKnownAsAsync(userId, knownAs);

    public Task UpdateDuckCharacterAsync(Guid userId, DuckCharacter character) =>
        _authService.UpdateDuckCharacterAsync(userId, character);
}
