using DuckyEQ.Contracts.DTOs;
using DuckyEQ.Contracts.Interfaces.Repositories;
using DuckyEQ.Contracts.Interfaces.Services;
using DuckyEQ.Contracts.Models;
using DuckyEQ.Domain.Entities;
using DuckyEQ.Domain.Enums;
using DuckyEQ.Services.Exceptions;

namespace DuckyEQ.Services.Services;

public class QuackService : IQuackService
{
    private readonly IQuackRepository _quackRepo;
    private readonly IFriendshipRepository _friendshipRepo;
    private readonly IUserRepository _userRepo;

    public QuackService(
        IQuackRepository quackRepo,
        IFriendshipRepository friendshipRepo,
        IUserRepository userRepo)
    {
        _quackRepo = quackRepo;
        _friendshipRepo = friendshipRepo;
        _userRepo = userRepo;
    }

    public async Task<QuackDto> SendQuackAsync(Guid senderId, SendQuackRequest request)
    {
        var friendship = await _friendshipRepo.GetBetweenUsersAsync(senderId, request.RecipientId);
        if (friendship is null || friendship.Status != FriendshipStatus.Accepted)
            throw new NotFriendsException();

        if (await _quackRepo.HasSentTodayAsync(senderId, request.RecipientId))
            throw new QuackLimitExceededException();

        var quack = Quack.Create(senderId, request.RecipientId, request.QuackType);
        var created = await _quackRepo.CreateAsync(quack);

        var sender = await _userRepo.GetByIdAsync(senderId)
            ?? throw new NotFoundException("Sender not found.");
        return ToDto(created, sender);
    }

    public async Task<IReadOnlyList<QuackDto>> GetUnseenAsync(Guid userId)
    {
        var quacks = await _quackRepo.GetUnseenByRecipientAsync(userId);
        var result = new List<QuackDto>();

        foreach (var q in quacks)
        {
            var sender = await _userRepo.GetByIdAsync(q.SenderId);
            if (sender is not null)
                result.Add(ToDto(q, sender));
        }

        return result;
    }

    public async Task MarkSeenAsync(Guid userId, Guid quackId)
    {
        // Validate the quack belongs to this recipient and is still unseen
        var unseen = await _quackRepo.GetUnseenByRecipientAsync(userId);
        if (!unseen.Any(q => q.Id == quackId))
            throw new NotFoundException("Quack not found or already marked as seen.");

        await _quackRepo.MarkSeenAsync(quackId);
    }

    private static QuackDto ToDto(Quack q, User sender) =>
        new(q.Id, q.SenderId, sender.KnownAs, sender.Username, sender.DuckCharacter,
            q.RecipientId, q.QuackType, q.SentAt, q.SeenAt);
}
