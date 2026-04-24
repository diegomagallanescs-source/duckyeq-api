using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuckyEQ.Contracts.DTOs;
using DuckyEQ.Contracts.Models;

namespace DuckyEQ.Contracts.Interfaces.Services
{
    internal interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterRequest request);
        Task<AuthResult> LoginAsync(LoginRequest request);
        Task<bool> VerifyEmailAsync(string token);
        // Re-issues JWT with updated KnownAs claim after DB save
        Task<AuthResult> UpdateKnownAsAsync(Guid userId, string knownAs);

    }
}
