using DuckyEQ.Contracts.Interfaces.Services;

namespace DuckyEQ.Services.Services;

public class ScoringService : IScoringService
{
    public int CalculateScore(int correctAnswers, int totalQuestions)
    {
        if (totalQuestions <= 0) return 0;
        var score = (int)Math.Round((double)correctAnswers / totalQuestions * 300);
        return Math.Clamp(score, 0, 300);
    }

    public int GetStars(int score) => score switch
    {
        <= 100 => 1,
        <= 200 => 2,
        _ => 3
    };

    public int BaseCoinsForStars(int stars) => stars switch
    {
        1 => 20,
        2 => 40,
        _ => 60
    };
}
