using DuckyEQ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.Interfaces.Repositories
{
    public interface IGratitudeRepository
    {
        Task<GratitudeEntry> CreateAsync(GratitudeEntry entry);
        Task<IReadOnlyList<GratitudeEntry>> GetByUserPagedAsync(
            Guid userId, int page, int pageSize);
        Task<IReadOnlyList<GratitudeEntry>> GetTodayByUserAsync(Guid userId);
        Task<GratitudeEntry?> GetRandomByUserAsync(Guid userId);
        Task<int> GetCurrentStreakAsync(Guid userId);
        Task<int> GetLongestStreakAsync(Guid userId);

    }
}

