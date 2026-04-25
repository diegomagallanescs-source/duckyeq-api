using DuckyEQ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.DTOs
{
    public record ShopItemDto(
        Guid Id,
        string Name,
        ShopCategory Category,
        int CoinPrice,
        string? DuckyImageUrl,
        string? DaisyImageUrl,
        bool IsOwned,
        bool IsEquipped,
        string? Rarity,
        string? Description
    );

}

