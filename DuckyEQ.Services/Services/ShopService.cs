using DuckyEQ.Contracts.DTOs;
using DuckyEQ.Contracts.Interfaces.Repositories;
using DuckyEQ.Contracts.Interfaces.Services;
using DuckyEQ.Contracts.Models;
using DuckyEQ.Domain.Entities;
using DuckyEQ.Services.Exceptions;

namespace DuckyEQ.Services.Services;

public class ShopService : IShopService
{
    private readonly IShopItemRepository _shopRepo;
    private readonly IUserInventoryRepository _inventoryRepo;
    private readonly ICoinService _coinService;

    public ShopService(
        IShopItemRepository shopRepo,
        IUserInventoryRepository inventoryRepo,
        ICoinService coinService)
    {
        _shopRepo = shopRepo;
        _inventoryRepo = inventoryRepo;
        _coinService = coinService;
    }

    public async Task<IReadOnlyList<ShopItemDto>> GetActiveItemsAsync()
    {
        var items = await _shopRepo.GetActiveItemsAsync();
        return items.Select(s => new ShopItemDto(
            s.Id, s.Name, s.Category, s.CoinPrice,
            s.DuckyImageUrl, s.DaisyImageUrl,
            false, false, s.Rarity, s.Description)).ToList();
    }

    public async Task<PurchaseResponse> PurchaseAsync(Guid userId, Guid shopItemId)
    {
        var item = await _shopRepo.GetByIdAsync(shopItemId)
            ?? throw new NotFoundException("Shop item not found.");

        if (await _inventoryRepo.UserOwnsItemAsync(userId, shopItemId))
            throw new AlreadyOwnedException();

        if (!await _coinService.TryDeductAsync(userId, item.CoinPrice))
            throw new InsufficientCoinsException();

        var inventory = UserInventory.Create(userId, shopItemId, item.Category);
        await _inventoryRepo.CreateAsync(inventory);

        // Auto-equip on purchase (unequip current slot first, then equip new)
        await _inventoryRepo.UnequipAllInCategoryAsync(userId, item.Category);
        await _inventoryRepo.EquipAsync(userId, shopItemId);

        var equipped = await _inventoryRepo.GetEquippedItemsAsync(userId);
        var newBalance = await _coinService.GetBalanceAsync(userId);

        return new PurchaseResponse(true, newBalance, equipped);
    }

    public async Task<EquippedItems> EquipAsync(Guid userId, Guid shopItemId)
    {
        var item = await _shopRepo.GetByIdAsync(shopItemId)
            ?? throw new NotFoundException("Shop item not found.");

        if (!await _inventoryRepo.UserOwnsItemAsync(userId, shopItemId))
            throw new NotFoundException("Item not in inventory.");

        await _inventoryRepo.UnequipAllInCategoryAsync(userId, item.Category);
        await _inventoryRepo.EquipAsync(userId, shopItemId);

        return await _inventoryRepo.GetEquippedItemsAsync(userId);
    }

    public async Task<IReadOnlyList<UserInventoryDto>> GetInventoryAsync(Guid userId)
    {
        var inventories = await _inventoryRepo.GetByUserAsync(userId);
        var result = new List<UserInventoryDto>();

        foreach (var inv in inventories)
        {
            var shopItem = await _shopRepo.GetByIdAsync(inv.ShopItemId);
            result.Add(new UserInventoryDto(
                inv.Id, inv.ShopItemId,
                shopItem?.Name ?? string.Empty,
                inv.Category, inv.IsEquipped, inv.PurchasedAt));
        }

        return result;
    }
}
