using DuckyEQ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.Interfaces.Repositories
{
    internal interface IShopItemRepository
    {
        Task<IReadOnlyList<ShopItem>> GetActiveItemsAsync();
        Task<IReadOnlyList<ShopItem>> GetWeeklyItemsForCurrentWeekAsync();
        Task<IReadOnlyList<ShopItem>> GetByTypeAsync(bool isWeekly);
        Task<ShopItem?> GetByIdAsync(Guid id);

    }
}
