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
}