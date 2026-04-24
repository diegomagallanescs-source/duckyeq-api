using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.DTOs
{
    public record CheckInDto(
        Guid Id,
        Guid UserId,
        DateOnly CheckInDate,       // UTC date only — no time component
        List<string> EmotionIds,      // ["Happy", "Anxious"] — multi-select
        string? Phrase,            // null = user chose not to share
        DateTime CreatedAt
    );
}
