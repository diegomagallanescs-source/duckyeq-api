using DuckyEQ.Contracts.DTOs;
using DuckyEQ.Contracts.Interfaces.Repositories;
using DuckyEQ.Domain.Entities;
using DuckyEQ.Domain.Enums;
using DuckyEQ.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DuckyEQ.Infrastructure.Repositories;

public class SqlUserInventoryRepository : IUserInventoryRepository
{
    private readonly DuckyEQDbContext _db;
    public SqlUserInventoryRepository(DuckyEQDbContext db) => _db = db;

    public async Task<IReadOnlyList<UserInventory>> GetByUserAsync(Guid userId) =>
        await _db.UserInventories.Where(i => i.UserId == userId).ToListAsync();

    public Task<UserInventory?> GetByUserAndItemAsync(Guid userId, Guid shopItemId) =>
        _db.UserInventories.FirstOrDefaultAsync(i => i.UserId == userId && i.ShopItemId == shopItemId);

    public Task<bool> UserOwnsItemAsync(Guid userId, Guid shopItemId) =>
        _db.UserInventories.AnyAsync(i => i.UserId == userId && i.ShopItemId == shopItemId);

    public async Task<UserInventory> CreateAsync(UserInventory item)
    {
        _db.UserInventories.Add(item);
        await _db.SaveChangesAsync();
        return item;
    }

    public async Task UnequipAllInCategoryAsync(Guid userId, ShopCategory category)
    {
        var equipped = await _db.UserInventories
            .Where(i => i.UserId == userId && i.Category == category && i.IsEquipped)
            .ToListAsync();

        foreach (var item in equipped) item.Unequip();
        await _db.SaveChangesAsync();
    }

    public async Task EquipAsync(Guid userId, Guid shopItemId)
    {
        var item = await _db.UserInventories
            .FirstOrDefaultAsync(i => i.UserId == userId && i.ShopItemId == shopItemId);

        if (item is not null)
        {
            item.Equip();
            await _db.SaveChangesAsync();
        }
    }

    public async Task<EquippedItems> GetEquippedItemsAsync(Guid userId)
    {
        var equipped = await _db.UserInventories
            .Where(i => i.UserId == userId && i.IsEquipped)
            .Join(_db.ShopItems, i => i.ShopItemId, s => s.Id,
                  (i, s) => new { Inventory = i, ShopItem = s })
            .ToListAsync();

        ShopItemDto? ToDto(ShopCategory cat)
        {
            var entry = equipped.FirstOrDefault(x => x.Inventory.Category == cat);
            if (entry is null) return null;
            var s = entry.ShopItem;
            return new ShopItemDto(s.Id, s.Name, s.Category, s.CoinPrice,
                s.DuckyImageUrl, s.DaisyImageUrl, true, true, s.Rarity, s.Description);
        }

        return new EquippedItems(
            Hat: ToDto(ShopCategory.Hat),
            Accessory: ToDto(ShopCategory.Accessory),
            Glow: ToDto(ShopCategory.Glow),
            Color: ToDto(ShopCategory.Color)
        );
    }
}
