using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuckyEQ.Contracts.DTOs;
using DuckyEQ.Contracts.Models;
using DuckyEQ.Domain.Enums;

namespace DuckyEQ.Contracts.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterRequest request);
        Task<AuthResult> LoginAsync(LoginRequest request);
        Task<bool> VerifyEmailAsync(string token);
        Task<UserProfileDto> GetProfileAsync(Guid userId);
        Task<AuthResult> UpdateKnownAsAsync(Guid userId, string knownAs);
        Task UpdateDuckCharacterAsync(Guid userId, DuckCharacter character);

    }
}

