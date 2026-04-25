using DuckyEQ.Contracts.DTOs;
using DuckyEQ.Contracts.Interfaces.Services;
using DuckyEQ.Contracts.Models;

namespace DuckyEQ.Services.Behaviors;

public class CheckInBehavior
{
    private readonly ICheckInService _checkInService;

    public CheckInBehavior(ICheckInService checkInService)
    {
        _checkInService = checkInService;
    }

    public Task<CheckInDto?> GetTodayAsync(Guid userId) =>
        _checkInService.GetTodayAsync(userId);

    public Task<CheckInDto> CheckInAsync(Guid userId, CheckInRequest request) =>
        _checkInService.CheckInAsync(userId, request);
}
