using DuckyEQ.Contracts.DTOs;
using DuckyEQ.Contracts.Interfaces.Services;
using DuckyEQ.Contracts.Models;
using DuckyEQ.Domain.Enums;

namespace DuckyEQ.Services.Behaviors;

public class LessonBehavior
{
    private readonly ILessonService _lessonService;
    private readonly ICooldownService _cooldownService;

    public LessonBehavior(ILessonService lessonService, ICooldownService cooldownService)
    {
        _lessonService = lessonService;
        _cooldownService = cooldownService;
    }

    public Task<IReadOnlyList<PillarProgressDto>> GetAllPillarProgressAsync(Guid userId) =>
        _lessonService.GetAllPillarProgressAsync(userId);

    public Task<IReadOnlyList<LessonWithProgressDto>> GetLessonsForPillarAsync(Guid userId, Pillar pillar) =>
        _lessonService.GetLessonsForPillarAsync(userId, pillar);

    public Task<LessonContentDto> GetLessonContentAsync(Pillar pillar, int level) =>
        _lessonService.GetLessonContentAsync(pillar, level);

    public Task<StartLessonResult> StartLessonAsync(Guid userId, Guid lessonId) =>
        _lessonService.StartLessonAsync(userId, lessonId);

    public Task<LessonCompleteResult> CompleteLessonAsync(Guid userId, CompleteLessonRequest request) =>
        _lessonService.CompleteLessonAsync(userId, request);

    public Task<CooldownStatus> GetCooldownStatusAsync(Guid userId, Pillar pillar) =>
        _cooldownService.GetPillarStatusAsync(userId, pillar);
}
