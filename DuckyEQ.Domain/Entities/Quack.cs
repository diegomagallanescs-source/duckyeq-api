using DuckyEQ.Domain.Enums;

namespace DuckyEQ.Domain.Entities;

public class Quack
{
    public Guid Id { get; private set; }
    public Guid SenderId { get; private set; }
    public Guid RecipientId { get; private set; }
    public QuackType QuackType { get; private set; }
    public DateTime SentAt { get; private set; }
    public DateTime? SeenAt { get; private set; }

    private Quack() { }

    public static Quack Create(Guid senderId, Guid recipientId, QuackType quackType)
    {
        return new Quack
        {
            Id = Guid.NewGuid(),
            SenderId = senderId,
            RecipientId = recipientId,
            QuackType = quackType,
            SentAt = DateTime.UtcNow
        };
    }

    public void MarkSeen() => SeenAt = DateTime.UtcNow;
}