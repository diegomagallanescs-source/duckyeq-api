using DuckyEQ.Domain.Enums;

namespace DuckyEQ.Domain.Entities;

public class UserInventory
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid ShopItemId { get; private set; }
    public ShopCategory Category { get; private set; }   // denormalized for fast UnequipAllInCategoryAsync
    public bool IsEquipped { get; private set; }
    public DateTime PurchasedAt { get; private set; }

    private UserInventory() { }

    public static UserInventory Create(Guid userId, Guid shopItemId, ShopCategory category)
    {
        return new UserInventory
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ShopItemId = shopItemId,
            Category = category,
            IsEquipped = false,
            PurchasedAt = DateTime.UtcNow
        };
    }

    public void Equip() => IsEquipped = true;
    public void Unequip() => IsEquipped = false;
}
