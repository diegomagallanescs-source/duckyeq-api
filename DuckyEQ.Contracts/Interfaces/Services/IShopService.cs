using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuckyEQ.Contracts.DTOs;
using DuckyEQ.Contracts.Models;

namespace DuckyEQ.Contracts.Interfaces.Services
{
    internal interface IShopService
    {
        Task<IReadOnlyList<ShopItemDto>> GetActiveItemsAsync();

        // Validates ownership, deducts coins, creates UserInventory row
        // Throws InsufficientCoinsException (→ 402) if balance too low
        // Throws AlreadyOwnedException (→ 409) if user already owns the item
        Task<PurchaseResponse> PurchaseAsync(Guid userId, Guid shopItemId);

        // Runs UnequipAllInCategory + EquipAsync in a transaction
        // Validates user owns the item before equipping
        Task<EquippedItems> EquipAsync(Guid userId, Guid shopItemId);

        Task<IReadOnlyList<UserInventoryDto>> GetInventoryAsync(Guid userId);

    }
}
