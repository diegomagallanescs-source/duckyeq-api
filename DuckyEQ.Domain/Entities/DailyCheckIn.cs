using DuckyEQ.Domain.Enums;

namespace DuckyEQ.Domain.Entities;

public class DailyCheckIn
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public DateOnly CheckInDate { get; private set; }
    public Emotion EmotionId { get; private set; }
    public string? Phrase { get; private set; }        // null = intentionally not shared
    public DateTime CreatedAt { get; private set; }

    private DailyCheckIn() { }

    public static DailyCheckIn Create(Guid userId, DateOnly checkinDate, Emotion emotion, string? phrase)
    {
        // Empty string rejected — converted to null
        var cleanPhrase = string.IsNullOrWhiteSpace(phrase) ? null : phrase.Trim();

        return new DailyCheckIn
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CheckInDate = checkinDate,
            EmotionId = emotion,
            Phrase = cleanPhrase,
            CreatedAt = DateTime.UtcNow
        };
    }
}