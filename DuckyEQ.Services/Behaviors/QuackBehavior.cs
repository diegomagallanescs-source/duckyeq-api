using DuckyEQ.Contracts.DTOs;
using DuckyEQ.Contracts.Interfaces.Services;
using DuckyEQ.Contracts.Models;

namespace DuckyEQ.Services.Behaviors;

public class QuackBehavior
{
    private readonly IQuackService _quackService;

    public QuackBehavior(IQuackService quackService)
    {
        _quackService = quackService;
    }

    public Task<QuackDto> SendQuackAsync(Guid senderId, SendQuackRequest request) =>
        _quackService.SendQuackAsync(senderId, request);

    public Task<IReadOnlyList<QuackDto>> GetUnseenAsync(Guid userId) =>
        _quackService.GetUnseenAsync(userId);

    public Task MarkSeenAsync(Guid userId, Guid quackId) =>
        _quackService.MarkSeenAsync(userId, quackId);
}
