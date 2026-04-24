using DuckyEQ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.DTOs
{
    public record FriendDetailDto(
        Guid UserId,
        string KnownAs,
        string Username,
        int OverallLevel,
        int OverallXP,
        DuckCharacter DuckCharacter,
        EquippedItems EquippedItems,
        // Today's check-in (nullable — friend may not have checked in yet)
        bool HasCheckedInToday,
        List<string>? TodayEmotionIds,
        string? TodayPhrase,
        // Relationship context
        Guid FriendshipId
    );

}
