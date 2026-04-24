using DuckyEQ.Domain.Enums;

namespace DuckyEQ.Domain.Entities;

public class UserEQTestResult
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Pillar Pillar { get; private set; }
    public int Score { get; private set; }
    public int TotalQuestions { get; private set; }
    public DateTime CompletedAt { get; private set; }

    private UserEQTestResult() { }
}