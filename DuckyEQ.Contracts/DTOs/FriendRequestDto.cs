using DuckyEQ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.DTOs
{
    public record FriendRequestDto(
        Guid FriendshipId,
        Guid RequesterId,
        string RequesterKnownAs,
        string RequesterUsername,
        DuckCharacter RequesterDuckCharacter,
        EquippedItems RequesterEquippedItems,
        DateTime CreatedAt
    );

}
