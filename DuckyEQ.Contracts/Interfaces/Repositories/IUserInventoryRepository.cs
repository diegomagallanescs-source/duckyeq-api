using DuckyEQ.Domain.Entities;
using DuckyEQ.Domain.Enums;
using DuckyEQ.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.Interfaces.Repositories
{
    internal interface IUserInventoryRepository
    {
        Task<IReadOnlyList<UserInventory>> GetByUserAsync(Guid userId);
        Task<UserInventory?> GetByUserAndItemAsync(Guid userId, Guid shopItemId);
        Task<bool> UserOwnsItemAsync(Guid userId, Guid shopItemId);
        Task<UserInventory> CreateAsync(UserInventory item);
        Task UnequipAllInCategoryAsync(Guid userId, ShopCategory category);
        Task EquipAsync(Guid userId, Guid shopItemId);
        Task<EquippedItems> GetEquippedItemsAsync(Guid userId);

    }
}
