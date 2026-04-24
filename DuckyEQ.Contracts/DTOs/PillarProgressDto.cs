using DuckyEQ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.DTOs
{
    public record PillarProgressDto(
        Pillar Pillar,
        string Name,
        int CurrentLevel,
        int XP,
        bool IsUnlocked
    );

}
