using DuckyEQ.Domain.Enums;

namespace DuckyEQ.Domain.Entities;

public class PillarProgress
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Pillar Pillar { get; private set; }
    public int CurrentLevel { get; private set; }
    public int XP { get; private set; }
    public DateTime? LastNewLessonCompletedAt { get; private set; }

    private PillarProgress() { }

    public static PillarProgress Create(Guid userId, Pillar pillar)
    {
        return new PillarProgress
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Pillar = pillar,
            CurrentLevel = 1,
            XP = 0
        };
    }

    public void AddXP(int amount) => XP += amount;
    public void SetLevel(int level) => CurrentLevel = level;
    public void RecordLessonCompletion() => LastNewLessonCompletedAt = DateTime.UtcNow;
}
