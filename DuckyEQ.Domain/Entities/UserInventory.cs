using DuckyEQ.Domain.Enums;

namespace DuckyEQ.Domain.Entities;
public class UserInventory
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid ShopItemId { get; private set; }
    public DateTime AcquiredAt { get; private set; }
    public bool IsEquipped { get; private set; }

    private UserInventory() { }
}