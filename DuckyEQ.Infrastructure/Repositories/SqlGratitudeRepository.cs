using DuckyEQ.Contracts.Interfaces.Repositories;
using DuckyEQ.Domain.Entities;
using DuckyEQ.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DuckyEQ.Infrastructure.Repositories;

public class SqlGratitudeRepository : IGratitudeRepository
{
    private readonly DuckyEQDbContext _db;
    public SqlGratitudeRepository(DuckyEQDbContext db) => _db = db;

    public async Task<GratitudeEntry> CreateAsync(GratitudeEntry entry)
    {
        _db.GratitudeEntries.Add(entry);
        await _db.SaveChangesAsync();
        return entry;
    }

    public async Task<IReadOnlyList<GratitudeEntry>> GetByUserPagedAsync(Guid userId, int page, int pageSize) =>
        await _db.GratitudeEntries
            .Where(g => g.UserId == userId)
            .OrderByDescending(g => g.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<IReadOnlyList<GratitudeEntry>> GetTodayByUserAsync(Guid userId)
    {
        var today = DateTime.UtcNow.Date;
        return await _db.GratitudeEntries
            .Where(g => g.UserId == userId && g.CreatedAt.Date == today)
            .ToListAsync();
    }

    public Task<GratitudeEntry?> GetRandomByUserAsync(Guid userId) =>
        _db.GratitudeEntries
            .Where(g => g.UserId == userId)
            .OrderBy(_ => Guid.NewGuid())
            .FirstOrDefaultAsync();

    public async Task<int> GetCurrentStreakAsync(Guid userId)
    {
        var dates = await _db.GratitudeEntries
            .Where(g => g.UserId == userId)
            .Select(g => g.CreatedAt.Date)
            .Distinct()
            .OrderByDescending(d => d)
            .ToListAsync();

        if (dates.Count == 0) return 0;

        var today = DateTime.UtcNow.Date;

        // streak must start from today or yesterday (grace period)
        if (dates[0] != today && dates[0] != today.AddDays(-1))
            return 0;

        var streak = 0;
        var expected = dates[0];

        foreach (var date in dates)
        {
            if (date == expected) { streak++; expected = expected.AddDays(-1); }
            else break;
        }

        return streak;
    }

    public async Task<int> GetLongestStreakAsync(Guid userId)
    {
        var dates = await _db.GratitudeEntries
            .Where(g => g.UserId == userId)
            .Select(g => g.CreatedAt.Date)
            .Distinct()
            .OrderBy(d => d)
            .ToListAsync();

        if (dates.Count == 0) return 0;

        var longest = 1;
        var current = 1;

        for (int i = 1; i < dates.Count; i++)
        {
            if ((dates[i] - dates[i - 1]).Days == 1) { current++; longest = Math.Max(longest, current); }
            else current = 1;
        }

        return longest;
    }
}
