using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuckyEQ.Domain.Entities;
using DuckyEQ.Domain.Enums;

namespace DuckyEQ.Contracts.DTOs
{
    public record UserSearchResultDto(
        Guid UserId,
        string Username,
        string KnownAs,
        DuckCharacter DuckCharacter,
        EquippedItems EquippedItems,
        // Existing relationship status (null = no relationship yet)
        FriendshipStatus? ExistingRelationship
    );

}
