using DuckyEQ.Contracts.Interfaces.Repositories;
using DuckyEQ.Contracts.Interfaces.Services;
using DuckyEQ.Contracts.Models;
using DuckyEQ.Domain.Enums;

namespace DuckyEQ.Services.Services;

public class CooldownService : ICooldownService
{
    private readonly IUserLessonProgressRepository _progressRepo;
    private static readonly TimeSpan CooldownWindow = TimeSpan.FromHours(24);

    public CooldownService(IUserLessonProgressRepository progressRepo)
    {
        _progressRepo = progressRepo;
    }

    public async Task<CooldownStatus> GetPillarStatusAsync(Guid userId, Pillar pillar)
    {
        var lastCompletion = await _progressRepo.GetLastNewLessonCompletedAtAsync(userId, pillar);
        if (lastCompletion is null)
            return new CooldownStatus(false, null);

        var nextAvailable = lastCompletion.Value + CooldownWindow;
        var isLocked = DateTime.UtcNow < nextAvailable;
        return new CooldownStatus(isLocked, isLocked ? nextAvailable : null);
    }

    public async Task<bool> CanStartNewLessonAsync(Guid userId, Pillar pillar)
    {
        var status = await GetPillarStatusAsync(userId, pillar);
        return !status.IsLocked;
    }
}
