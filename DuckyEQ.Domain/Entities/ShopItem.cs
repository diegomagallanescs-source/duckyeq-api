using DuckyEQ.Domain.Enums;

namespace DuckyEQ.Domain.Entities;
public class ShopItem
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;
    public int CoinCost { get; private set; }
    public string ImageUrl { get; private set; } = string.Empty;
    public DateTime AvailableFrom { get; private set; }
    public DateTime AvailableTo { get; private set; }

    private ShopItem() { }
}