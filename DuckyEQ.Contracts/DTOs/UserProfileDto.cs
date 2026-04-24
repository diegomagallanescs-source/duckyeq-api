using DuckyEQ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.DTOs
{
    public record UserProfileDto(
        string Username,          // PascalCase, immutable, shown in social UI
        string KnownAs,           // display name, max 10 chars, mutable
        DuckCharacter DuckCharacter,  // Ducky or Daisy
        int OverallXP,
        int OverallLevel,
        int StreakDays,
        bool EmailVerified,
        EquippedItems EquippedItems   // hat + accessory + glow + color
    );

}
