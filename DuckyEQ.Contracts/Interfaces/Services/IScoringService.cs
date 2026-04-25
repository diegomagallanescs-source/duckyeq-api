using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.Interfaces.Services
{
    public interface IScoringService
    {
        // score = round(correctAnswers / totalQuestions * 300), clamped 0–300
        int CalculateScore(int correctAnswers, int totalQuestions);

        // 1 star: score <= 100  |  2 stars: score <= 200  |  3 stars: score > 200
        int GetStars(int score);

        // Coin awards: 1 star = 20, 2 stars = 40, 3 stars = 60
        int BaseCoinsForStars(int stars);

    }
}

