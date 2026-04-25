using DuckyEQ.Contracts.Interfaces.Repositories;
using DuckyEQ.Domain.Entities;
using DuckyEQ.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DuckyEQ.Infrastructure.Repositories;

public class SqlDailyCheckInRepository : IDailyCheckInRepository
{
    private readonly DuckyEQDbContext _db;
    public SqlDailyCheckInRepository(DuckyEQDbContext db) => _db = db;

    // Returns null = user has not checked in today → controller returns 204 No Content
    public Task<DailyCheckIn?> GetTodayAsync(Guid userId)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        return _db.DailyCheckIns.FirstOrDefaultAsync(c => c.UserId == userId && c.CheckInDate == today);
    }

    public async Task<DailyCheckIn> CreateAsync(DailyCheckIn checkIn)
    {
        _db.DailyCheckIns.Add(checkIn);
        await _db.SaveChangesAsync();
        return checkIn;
    }
}
