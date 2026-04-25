using DuckyEQ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.Interfaces.Repositories
{
    public interface ICoinRepository
    {
        Task<QuackCoins?> GetByUserAsync(Guid userId);
        Task<QuackCoins> EnsureExistsAsync(Guid userId);
        Task AwardAsync(Guid userId, int amount);
        Task<bool> DeductAsync(Guid userId, int amount);

    }
}

