using DuckyEQ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.DTOs
{
    public record LessonWithProgressDto(
        Guid Id,
        Pillar Pillar,
        int Level,
        string Title,
        string Objective,
        int? BestScore,
        int? BestStars,
        DateTime? FirstCompletedAt,
        bool IsUnlocked
    );

}
