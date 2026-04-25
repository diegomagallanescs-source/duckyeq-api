using DuckyEQ.Domain.Enums;

namespace DuckyEQ.Domain.Entities;

public class GratitudeEntry
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Text { get; private set; } = null!;
    public GratitudeCategory Category { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private GratitudeEntry() { }

    public static GratitudeEntry Create(Guid userId, string text, GratitudeCategory category)
    {
        return new GratitudeEntry
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Text = text,
            Category = category,
            CreatedAt = DateTime.UtcNow
        };
    }
}
