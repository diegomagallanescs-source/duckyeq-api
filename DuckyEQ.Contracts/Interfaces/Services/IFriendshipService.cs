using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuckyEQ.Contracts.DTOs;

namespace DuckyEQ.Contracts.Interfaces.Services
{
    internal interface IFriendshipService
    {
        // Looks up target by username. Throws if already friends or request exists.
        Task<FriendshipDto> SendRequestAsync(Guid requesterId, string targetUsername);

        Task<IReadOnlyList<FriendRequestDto>> GetPendingIncomingAsync(Guid userId);

        // Validates that userId == AddresseeId before accepting
        Task<FriendshipDto> AcceptRequestAsync(Guid userId, Guid friendshipId);

        Task DeclineRequestAsync(Guid userId, Guid friendshipId);

        // Returns JOIN result: friend profile + today's check-in or null
        Task<IReadOnlyList<FriendWithCheckInDto>> GetFriendsWithCheckInAsync(
            Guid userId);

        Task<FriendDetailDto> GetFriendDetailAsync(
            Guid currentUserId, Guid friendUserId);

    }
}
