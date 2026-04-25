using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.Interfaces.Services
{
    public interface IEQTestScoringService
    {
        // score = correctAnswers * 10  (15 questions max = 150 max score)
        int CalculateScore(int correctAnswers);

        // 1 star: score < 60  |  2 stars: score < 110  |  3 stars: score >= 110
        int GetStars(int score);

    }
}

