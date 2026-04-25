using DuckyEQ.Contracts.Interfaces.Services;

namespace DuckyEQ.Services.Services;

public class EQTestScoringService : IEQTestScoringService
{
    public int CalculateScore(int correctAnswers) => correctAnswers * 10;

    public int GetStars(int score) => score switch
    {
        < 60 => 1,
        < 110 => 2,
        _ => 3
    };
}
