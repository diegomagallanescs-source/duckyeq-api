using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.DTOs;

public record GratitudeStreakDto(
    int CurrentStreak,
    int LongestStreak
);
