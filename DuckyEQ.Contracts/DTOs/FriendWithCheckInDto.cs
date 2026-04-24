using DuckyEQ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.DTOs
{
    public record FriendWithCheckInDto(
        Guid FriendshipId,
        Guid UserId,
        string KnownAs,
        string Username,
        int OverallLevel,
        DuckCharacter DuckCharacter,
        EquippedItems EquippedItems,
        // Check-in data — null values when HasCheckedInToday = false
        bool HasCheckedInToday,
        List<string>? EmotionIds,     // e.g. ["Happy", "Anxious"]
        string? Phrase             // max 120 chars, nullable
    );

}
