namespace DuckyEQ.Domain.Entities;

public class UserEQTestResult
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public int Score { get; private set; }
    public int Stars { get; private set; }              // 0–3
    public int CorrectAnswers { get; private set; }
    public DateTime AttemptedAt { get; private set; }

    private UserEQTestResult() { }

    public static UserEQTestResult Create(Guid userId, int score, int stars, int correctAnswers)
    {
        return new UserEQTestResult
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Score = score,
            Stars = stars,
            CorrectAnswers = correctAnswers,
            AttemptedAt = DateTime.UtcNow
        };
    }
}
