namespace DuckyEQ.Domain.Entities;

public class UserLessonProgress
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid LessonId { get; private set; }
    public int BestScore { get; private set; }          // 0–300
    public int BestStars { get; private set; }          // 1–3
    public int TotalAttempts { get; private set; }
    public DateTime? FirstCompletedAt { get; private set; }
    public DateTime LastAttemptedAt { get; private set; }

    private UserLessonProgress() { }

    public static UserLessonProgress Create(Guid userId, Guid lessonId, int score, int stars)
    {
        var now = DateTime.UtcNow;
        return new UserLessonProgress
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            LessonId = lessonId,
            BestScore = score,
            BestStars = stars,
            TotalAttempts = 1,
            FirstCompletedAt = now,
            LastAttemptedAt = now
        };
    }

    public void RecordAttempt(int score, int stars)
    {
        TotalAttempts++;
        LastAttemptedAt = DateTime.UtcNow;
        if (score > BestScore)
        {
            BestScore = score;
            BestStars = stars;
        }
    }
}
