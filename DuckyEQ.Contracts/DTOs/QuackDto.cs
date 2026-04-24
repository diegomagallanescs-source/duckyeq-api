using DuckyEQ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.DTOs
{
    public record QuackDto(
        Guid Id,
        Guid SenderId,
        string SenderKnownAs,
        string SenderUsername,
        DuckCharacter SenderDuckCharacter,
        Guid RecipientId,
        QuackType QuackType,         // Hug | Smile | HighFive | ThinkingOfYou | Cheer
        DateTime SentAt,
        DateTime? SeenAt             // null = unseen
    );

}
