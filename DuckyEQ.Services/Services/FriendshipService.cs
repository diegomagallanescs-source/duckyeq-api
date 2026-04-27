using DuckyEQ.Contracts.DTOs;
using DuckyEQ.Contracts.Interfaces.Repositories;
using DuckyEQ.Contracts.Interfaces.Services;
using DuckyEQ.Domain.Entities;
using DuckyEQ.Domain.Enums;
using DuckyEQ.Services.Exceptions;

namespace DuckyEQ.Services.Services;

public class FriendshipService : IFriendshipService
{
    private readonly IFriendshipRepository _friendshipRepo;
    private readonly IUserRepository _userRepo;
    private readonly IUserInventoryRepository _inventoryRepo;

    public FriendshipService(
        IFriendshipRepository friendshipRepo,
        IUserRepository userRepo,
        IUserInventoryRepository inventoryRepo)
    {
        _friendshipRepo = friendshipRepo;
        _userRepo = userRepo;
        _inventoryRepo = inventoryRepo;
    }

    public async Task<FriendshipDto> SendRequestAsync(Guid requesterId, string targetUsername)
    {
        var target = await _userRepo.GetByUsernameAsync(targetUsername)
            ?? throw new NotFoundException("User not found.");

        if (target.Id == requesterId)
            throw new FriendRequestConflictException("You cannot send a friend request to yourself.");

        var existing = await _friendshipRepo.GetBetweenUsersAsync(requesterId, target.Id);
        if (existing is not null)
            throw new FriendRequestConflictException("A friendship or pending request already exists with this user.");

        var friendship = Friendship.Create(requesterId, target.Id);
        var created = await _friendshipRepo.CreateAsync(friendship);
        return ToDto(created);
    }

    public async Task<IReadOnlyList<FriendRequestDto>> GetPendingIncomingAsync(Guid userId)
    {
        var requests = await _friendshipRepo.GetPendingIncomingAsync(userId);
        var result = new List<FriendRequestDto>();

        foreach (var r in requests)
        {
            var requester = await _userRepo.GetByIdAsync(r.RequesterId);
            if (requester is null) continue;

            var equipped = await _inventoryRepo.GetEquippedItemsAsync(r.RequesterId);
            result.Add(new FriendRequestDto(
                r.Id, r.RequesterId,
                requester.KnownAs, requester.Username, requester.DuckCharacter,
                equipped, r.CreatedAt));
        }

        return result;
    }

    public async Task<FriendshipDto> AcceptRequestAsync(Guid userId, Guid friendshipId)
    {
        var friendship = await _friendshipRepo.GetByIdAsync(friendshipId)
            ?? throw new NotFoundException("Friendship request not found.");

        if (friendship.AddresseeId != userId)
            throw new UnauthorizedAccessException("Only the addressee can accept this request.");

        await _friendshipRepo.UpdateStatusAsync(friendshipId, FriendshipStatus.Accepted);
        var updated = await _friendshipRepo.GetByIdAsync(friendshipId);
        return ToDto(updated!);
    }

    public async Task DeclineRequestAsync(Guid userId, Guid friendshipId)
    {
        var friendship = await _friendshipRepo.GetByIdAsync(friendshipId)
            ?? throw new NotFoundException("Friendship request not found.");

        if (friendship.AddresseeId != userId)
            throw new UnauthorizedAccessException("Only the addressee can decline this request.");

        await _friendshipRepo.UpdateStatusAsync(friendshipId, FriendshipStatus.Declined);
    }

    public Task<IReadOnlyList<FriendWithCheckInDto>> GetFriendsWithCheckInAsync(Guid userId) =>
        _friendshipRepo.GetFriendsWithCheckInAsync(userId);

    public async Task<FriendDetailDto> GetFriendDetailAsync(Guid currentUserId, Guid friendUserId)
    {
        var detail = await _friendshipRepo.GetFriendDetailAsync(currentUserId, friendUserId)
            ?? throw new NotFoundException("Friend relationship not found.");
        return detail;
    }

    public async Task<IReadOnlyList<UserSearchResultDto>> SearchUsersAsync(Guid currentUserId, string prefix)
    {
        var users = await _userRepo.SearchByUsernameAsync(prefix, currentUserId);
        var result = new List<UserSearchResultDto>();

        foreach (var u in users)
        {
            var friendship = await _friendshipRepo.GetBetweenUsersAsync(currentUserId, u.Id);
            var equipped = await _inventoryRepo.GetEquippedItemsAsync(u.Id);
            result.Add(new UserSearchResultDto(
                u.Id, u.Username, u.KnownAs, u.DuckCharacter,
                equipped, friendship?.Status));
        }

        return result;
    }

    private static FriendshipDto ToDto(Friendship f) =>
        new(f.Id, f.RequesterId, f.AddresseeId, f.Status, f.CreatedAt);
}
