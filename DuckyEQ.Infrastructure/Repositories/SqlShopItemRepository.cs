using DuckyEQ.Contracts.Interfaces.Repositories;
using DuckyEQ.Domain.Entities;
using DuckyEQ.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DuckyEQ.Infrastructure.Repositories;

public class SqlShopItemRepository : IShopItemRepository
{
    private readonly DuckyEQDbContext _db;
    public SqlShopItemRepository(DuckyEQDbContext db) => _db = db;

    public async Task<IReadOnlyList<ShopItem>> GetActiveItemsAsync() =>
        await _db.ShopItems.Where(s => s.IsActive).ToListAsync();

    public async Task<IReadOnlyList<ShopItem>> GetWeeklyItemsForCurrentWeekAsync()
    {
        var now = DateTime.UtcNow;
        return await _db.ShopItems
            .Where(s => s.IsWeeklyItem && s.IsActive
                        && s.WeeklyAvailableFrom <= now
                        && s.WeeklyAvailableTo >= now)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<ShopItem>> GetByTypeAsync(bool isWeekly) =>
        await _db.ShopItems.Where(s => s.IsWeeklyItem == isWeekly && s.IsActive).ToListAsync();

    public Task<ShopItem?> GetByIdAsync(Guid id) =>
        _db.ShopItems.FirstOrDefaultAsync(s => s.Id == id);
}
