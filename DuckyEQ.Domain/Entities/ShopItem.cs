using DuckyEQ.Domain.Enums;

namespace DuckyEQ.Domain.Entities;

public class ShopItem
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public ShopCategory Category { get; private set; }
    public int CoinPrice { get; private set; }
    public string? DuckyImageUrl { get; private set; }
    public string? DaisyImageUrl { get; private set; }
    public bool IsDefault { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsWeeklyItem { get; private set; }
    public int? WeekNumber { get; private set; }
    public DateTime? WeeklyAvailableFrom { get; private set; }
    public DateTime? WeeklyAvailableTo { get; private set; }
    public string? Description { get; private set; }
    public string? Rarity { get; private set; }

    private ShopItem() { }

    public static ShopItem Create(
        string name,
        ShopCategory category,
        int coinPrice,
        string? duckyImageUrl = null,
        string? daisyImageUrl = null,
        bool isDefault = false,
        bool isActive = true,
        bool isWeeklyItem = false,
        int? weekNumber = null,
        DateTime? weeklyAvailableFrom = null,
        DateTime? weeklyAvailableTo = null,
        string? description = null,
        string? rarity = null)
    {
        return new ShopItem
        {
            Id = Guid.NewGuid(),
            Name = name,
            Category = category,
            CoinPrice = coinPrice,
            DuckyImageUrl = duckyImageUrl,
            DaisyImageUrl = daisyImageUrl,
            IsDefault = isDefault,
            IsActive = isActive,
            IsWeeklyItem = isWeeklyItem,
            WeekNumber = weekNumber,
            WeeklyAvailableFrom = weeklyAvailableFrom,
            WeeklyAvailableTo = weeklyAvailableTo,
            Description = description,
            Rarity = rarity
        };
    }
}
