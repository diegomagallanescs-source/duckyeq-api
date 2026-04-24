using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.Models;

public record LessonCompleteResult(
    int Score,
    int Stars,
    bool IsNewBest,
    bool IsFirstCompletion,
    int CoinsAwarded
);