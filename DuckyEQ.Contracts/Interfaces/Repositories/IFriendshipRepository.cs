using DuckyEQ.Contracts.DTOs;
using DuckyEQ.Domain.Entities;
using DuckyEQ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.Interfaces.Repositories
{
    public interface IFriendshipRepository
    {
        Task<Friendship> CreateAsync(Friendship friendship);

        // JOIN: friendships + users + daily_checkins LEFT JOIN for today's check-in
        Task<IReadOnlyList<FriendWithCheckInDto>> GetFriendsWithCheckInAsync(
            Guid userId);

        // Incoming requests where AddresseeId = userId AND Status = Pending
        Task<IReadOnlyList<Friendship>> GetPendingIncomingAsync(Guid userId);

        Task<Friendship?> GetByIdAsync(Guid id);
        Task UpdateStatusAsync(Guid friendshipId, FriendshipStatus status);

        // Used to check if a request already exists before sending another
        Task<Friendship?> GetBetweenUsersAsync(Guid userId1, Guid userId2);

        // Rich JOIN for the Friend Detail screen
        Task<FriendDetailDto?> GetFriendDetailAsync(
            Guid currentUserId, Guid friendUserId);

    }
}

