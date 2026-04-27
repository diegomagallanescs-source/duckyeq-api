using DuckyEQ.Contracts.DTOs;
using DuckyEQ.Contracts.Interfaces.Services;

namespace DuckyEQ.Services.Behaviors;

public class FriendsBehavior
{
    private readonly IFriendshipService _friendshipService;

    public FriendsBehavior(IFriendshipService friendshipService)
    {
        _friendshipService = friendshipService;
    }

    public Task<FriendshipDto> SendRequestAsync(Guid requesterId, string targetUsername) =>
        _friendshipService.SendRequestAsync(requesterId, targetUsername);

    public Task<IReadOnlyList<FriendRequestDto>> GetPendingIncomingAsync(Guid userId) =>
        _friendshipService.GetPendingIncomingAsync(userId);

    public Task<FriendshipDto> AcceptRequestAsync(Guid userId, Guid friendshipId) =>
        _friendshipService.AcceptRequestAsync(userId, friendshipId);

    public Task DeclineRequestAsync(Guid userId, Guid friendshipId) =>
        _friendshipService.DeclineRequestAsync(userId, friendshipId);

    public Task<IReadOnlyList<FriendWithCheckInDto>> GetFriendsWithCheckInAsync(Guid userId) =>
        _friendshipService.GetFriendsWithCheckInAsync(userId);

    public Task<FriendDetailDto> GetFriendDetailAsync(Guid currentUserId, Guid friendUserId) =>
        _friendshipService.GetFriendDetailAsync(currentUserId, friendUserId);

    public Task<IReadOnlyList<UserSearchResultDto>> SearchUsersAsync(Guid currentUserId, string prefix) =>
        _friendshipService.SearchUsersAsync(currentUserId, prefix);
}
