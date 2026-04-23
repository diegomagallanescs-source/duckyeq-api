using DuckyEQ.Domain.Enums;

namespace DuckyEQ.Domain.Entities;

public class Friendship
{
    public Guid Id { get; private set; }
    public Guid RequesterId { get; private set; }
    public Guid AddresseeId { get; private set; }
    public FriendshipStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Friendship() { }

    public static Friendship Create(Guid requesterId, Guid addresseeId)
    {
        return new Friendship
        {
            Id = Guid.NewGuid(),
            RequesterId = requesterId,
            AddresseeId = addresseeId,
            Status = FriendshipStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Accept() { Status = FriendshipStatus.Accepted; UpdatedAt = DateTime.UtcNow; }
    public void Decline() { Status = FriendshipStatus.Declined; UpdatedAt = DateTime.UtcNow; }
}