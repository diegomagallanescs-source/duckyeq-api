using DuckyEQ.Contracts.Interfaces.Repositories;
using DuckyEQ.Domain.Entities;
using DuckyEQ.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DuckyEQ.Infrastructure.Repositories;

public class SqlCoinRepository : ICoinRepository
{
    private readonly DuckyEQDbContext _db;
    public SqlCoinRepository(DuckyEQDbContext db) => _db = db;

    public Task<QuackCoins?> GetByUserAsync(Guid userId) =>
        _db.QuackCoins.FirstOrDefaultAsync(c => c.UserId == userId);

    public async Task<QuackCoins> EnsureExistsAsync(Guid userId)
    {
        var coins = await _db.QuackCoins.FirstOrDefaultAsync(c => c.UserId == userId);
        if (coins is not null) return coins;

        coins = QuackCoins.Create(userId);
        _db.QuackCoins.Add(coins);
        await _db.SaveChangesAsync();
        return coins;
    }

    public async Task AwardAsync(Guid userId, int amount)
    {
        var coins = await EnsureExistsAsync(userId);
        coins.Award(amount);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeductAsync(Guid userId, int amount)
    {
        var coins = await _db.QuackCoins.FirstOrDefaultAsync(c => c.UserId == userId);
        if (coins is null || !coins.TryDeduct(amount)) return false;
        await _db.SaveChangesAsync();
        return true;
    }
}
