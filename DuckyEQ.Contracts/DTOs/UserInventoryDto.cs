using DuckyEQ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.DTOs
{
    public record UserInventoryDto(
        Guid Id,
        Guid ShopItemId,
        string ItemName,
        ShopCategory Category,
        bool IsEquipped,
        DateTime PurchasedAt
    );

}
