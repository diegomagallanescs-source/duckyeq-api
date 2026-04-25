using DuckyEQ.Contracts.DTOs;
using DuckyEQ.Contracts.Interfaces.Repositories;
using DuckyEQ.Contracts.Interfaces.Services;
using DuckyEQ.Contracts.Models;
using DuckyEQ.Domain.Entities;

namespace DuckyEQ.Services.Services;

public class GratitudeService : IGratitudeService
{
    private readonly IGratitudeRepository _gratitudeRepo;
    private readonly ICoinService _coinService;

    public GratitudeService(IGratitudeRepository gratitudeRepo, ICoinService coinService)
    {
        _gratitudeRepo = gratitudeRepo;
        _coinService = coinService;
    }

    public async Task<GratitudeResponse> AddEntryAsync(Guid userId, AddGratitudeRequest request)
    {
        var entry = GratitudeEntry.Create(userId, request.Text, request.Category);
        await _gratitudeRepo.CreateAsync(entry);

        // 10 coins on first entry of the day only
        var todayEntries = await _gratitudeRepo.GetTodayByUserAsync(userId);
        var coinsAwarded = 0;
        if (todayEntries.Count == 1)
        {
            await _coinService.AwardAsync(userId, 10);
            coinsAwarded = 10;
        }

        return new GratitudeResponse(entry.Id, coinsAwarded);
    }

    public async Task<IReadOnlyList<GratitudeEntryDto>> GetAllPagedAsync(Guid userId, int page, int pageSize)
    {
        var entries = await _gratitudeRepo.GetByUserPagedAsync(userId, page, pageSize);
        return entries.Select(ToDto).ToList();
    }

    public async Task<GratitudeEntryDto?> GetRandomAsync(Guid userId)
    {
        // Guard: need at least 3 total entries for Pick Me Up
        var sample = await _gratitudeRepo.GetByUserPagedAsync(userId, 1, 3);
        if (sample.Count < 3) return null;

        var entry = await _gratitudeRepo.GetRandomByUserAsync(userId);
        return entry is null ? null : ToDto(entry);
    }

    public async Task<GratitudeStreakDto> GetStreakAsync(Guid userId)
    {
        var current = await _gratitudeRepo.GetCurrentStreakAsync(userId);
        var longest = await _gratitudeRepo.GetLongestStreakAsync(userId);
        return new GratitudeStreakDto(current, longest);
    }

    private static GratitudeEntryDto ToDto(GratitudeEntry e) =>
        new(e.Id, e.Text, e.Category, e.CreatedAt);
}
