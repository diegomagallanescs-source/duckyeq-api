using DuckyEQ.Domain.Enums;

namespace DuckyEQ.Domain.Entities;
public class UserLessonProgress
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid LessonId { get; private set; }
    public int BestScore { get; private set; }        // 0–300
    public int BestStars { get; private set; }        // 1–3
    public int TotalAttempts { get; private set; }
    public DateTime? FirstCompletedAt { get; private set; }
    public DateTime LastAttemptedAt { get; private set; }

    private UserLessonProgress() { }
}