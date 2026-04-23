using DuckyEQ.Domain.Enums;

namespace DuckyEQ.Domain.Entities;
public class GratitudeEntry
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Content { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private GratitudeEntry() { }
}