using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.DTOs;

public record EQTestResultDto(
    int Score,
    int Stars,
    int CorrectAnswers,
    bool IsNewBest
);