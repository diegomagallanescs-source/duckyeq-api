using DuckyEQ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.DTOs
{
    public record GratitudeEntryDto(
        Guid Id,
        string Text,
        GratitudeCategory Category,
        DateTime CreatedAt
    );

}
