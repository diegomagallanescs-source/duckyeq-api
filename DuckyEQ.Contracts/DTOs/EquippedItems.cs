using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.DTOs
{
    public record EquippedItems(
        ShopItemDto? Hat,
        ShopItemDto? Accessory,
        ShopItemDto? Glow,
        ShopItemDto? Color
    );

}
