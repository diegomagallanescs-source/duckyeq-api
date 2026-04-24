using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.Interfaces.Services
{
    internal interface ICoinService
    {
        Task<int> GetBalanceAsync(Guid userId);
        Task AwardAsync(Guid userId, int amount);

        // Returns false when balance is insufficient — no throw
        Task<bool> TryDeductAsync(Guid userId, int amount);

    }
}
