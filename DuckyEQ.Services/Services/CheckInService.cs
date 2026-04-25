using DuckyEQ.Contracts.DTOs;
using DuckyEQ.Contracts.Interfaces.Repositories;
using DuckyEQ.Contracts.Interfaces.Services;
using DuckyEQ.Contracts.Models;
using DuckyEQ.Domain.Entities;
using DuckyEQ.Domain.Enums;
using DuckyEQ.Services.Exceptions;

namespace DuckyEQ.Services.Services;

public class CheckInService : ICheckInService
{
    private readonly IDailyCheckInRepository _checkInRepo;

    public CheckInService(IDailyCheckInRepository checkInRepo)
    {
        _checkInRepo = checkInRepo;
    }

    public async Task<CheckInDto?> GetTodayAsync(Guid userId)
    {
        var checkIn = await _checkInRepo.GetTodayAsync(userId);
        return checkIn is null ? null : ToDto(checkIn);
    }

    public async Task<CheckInDto> CheckInAsync(Guid userId, CheckInRequest request)
    {
        var existing = await _checkInRepo.GetTodayAsync(userId);
        if (existing is not null)
            throw new AlreadyCheckedInException();

        // Bridge: contract still uses List<string> (pending Day ~9 update to single EmotionId)
        var emotionString = request.EmotionIds.FirstOrDefault()
            ?? throw new ArgumentException("At least one emotion is required.");

        if (!Enum.TryParse<Emotion>(emotionString, ignoreCase: true, out var emotion))
            throw new ArgumentException($"Invalid emotion value: '{emotionString}'.");

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var checkIn = DailyCheckIn.Create(userId, today, emotion, request.Phrase);
        var created = await _checkInRepo.CreateAsync(checkIn);
        return ToDto(created);
    }

    private static CheckInDto ToDto(DailyCheckIn c) =>
        new(c.Id, c.UserId, c.CheckInDate, [c.EmotionId.ToString()], c.Phrase, c.CreatedAt);
}
