using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuckyEQ.Domain.Entities;

namespace DuckyEQ.Contracts.Interfaces.Repositories
{
    internal interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<bool> IsUsernameTakenAsync(string username);
        Task<User> CreateAsync(User user);
        Task UpdateAsync(User user);
        Task UpdateKnownAsAsync(Guid userId, string knownAs);

    }
}
