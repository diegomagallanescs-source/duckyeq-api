using DuckyEQ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.Interfaces.Repositories
{
    internal interface IDailyCheckInRepository
    {
        // Returns null if user has not checked in today — signals 204 No Content at API layer
        Task<DailyCheckIn?> GetTodayAsync(Guid userId);
        Task<DailyCheckIn> CreateAsync(DailyCheckIn checkIn);

    }
}
