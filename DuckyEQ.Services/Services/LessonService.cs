using DuckyEQ.Contracts.DTOs;
using DuckyEQ.Contracts.Interfaces.Repositories;
using DuckyEQ.Contracts.Interfaces.Services;
using DuckyEQ.Contracts.Models;
using DuckyEQ.Domain.Enums;
using DuckyEQ.Services.Exceptions;

namespace DuckyEQ.Services.Services;

public class LessonService : ILessonService
{
    private readonly ILessonRepository _lessonRepo;
    private readonly IPillarProgressRepository _pillarRepo;
    private readonly IUserLessonProgressRepository _progressRepo;
    private readonly IScoringService _scoring;
    private readonly ICoinService _coinService;
    private readonly ICooldownService _cooldown;
    private readonly ISessionService _session;

    public LessonService(
        ILessonRepository lessonRepo,
        IPillarProgressRepository pillarRepo,
        IUserLessonProgressRepository progressRepo,
        IScoringService scoring,
        ICoinService coinService,
        ICooldownService cooldown,
        ISessionService session)
    {
        _lessonRepo = lessonRepo;
        _pillarRepo = pillarRepo;
        _progressRepo = progressRepo;
        _scoring = scoring;
        _coinService = coinService;
        _cooldown = cooldown;
        _session = session;
    }

    public async Task<IReadOnlyList<PillarProgressDto>> GetAllPillarProgressAsync(Guid userId)
    {
        await _pillarRepo.EnsureAllPillarsExistAsync(userId);
        var progresses = await _pillarRepo.GetAllByUserAsync(userId);

        return progresses
            .OrderBy(p => (int)p.Pillar)
            .Select(p => new PillarProgressDto(
                p.Pillar,
                PillarDisplayName(p.Pillar),
                p.CurrentLevel,
                p.XP,
                IsUnlocked: true))
            .ToList();
    }

    public async Task<IReadOnlyList<LessonWithProgressDto>> GetLessonsForPillarAsync(Guid userId, Pillar pillar)
    {
        var lessons = await _lessonRepo.GetByPillarAsync(pillar);
        var progresses = await _progressRepo.GetByUserAndPillarAsync(userId, pillar);
        var progressMap = progresses.ToDictionary(p => p.LessonId);

        var completedIds = progressMap
            .Where(kvp => kvp.Value.FirstCompletedAt.HasValue)
            .Select(kvp => kvp.Key)
            .ToHashSet();

        var sortedLessons = lessons.OrderBy(l => l.Level).ToList();
        var lessonByLevel = sortedLessons.ToDictionary(l => l.Level);

        return sortedLessons.Select(l =>
        {
            progressMap.TryGetValue(l.Id, out var progress);

            bool isUnlocked = l.Level == 1 ||
                (lessonByLevel.TryGetValue(l.Level - 1, out var prev) && completedIds.Contains(prev.Id));

            return new LessonWithProgressDto(
                l.Id, l.PillarId, l.Level, l.Title, l.Objective,
                progress?.BestScore,
                progress?.BestStars,
                progress?.FirstCompletedAt,
                isUnlocked);
        }).ToList();
    }

    public async Task<LessonContentDto> GetLessonContentAsync(Pillar pillar, int level)
    {
        var lesson = await _lessonRepo.GetByPillarAndLevelAsync(pillar, level)
            ?? throw new NotFoundException($"Lesson not found for pillar {pillar}, level {level}.");

        return new LessonContentDto(lesson.Id, lesson.PillarId, lesson.Level, lesson.Title,
            lesson.EngageConfigJson);
    }

    public async Task<StartLessonResult> StartLessonAsync(Guid userId, Guid lessonId)
    {
        var lesson = await _lessonRepo.GetByIdAsync(lessonId)
            ?? throw new NotFoundException("Lesson not found.");

        // Cooldown only blocks first attempts — retries are always allowed
        var existingProgress = await _progressRepo.GetByUserAndLessonAsync(userId, lessonId);
        var isFirstAttempt = existingProgress is null;

        if (isFirstAttempt && !await _cooldown.CanStartNewLessonAsync(userId, lesson.PillarId))
            throw new CooldownActiveException();

        var token = _session.CreateSession(userId, lessonId);
        return new StartLessonResult(token, DateTime.UtcNow.AddMinutes(30));
    }

    public async Task<LessonCompleteResult> CompleteLessonAsync(Guid userId, CompleteLessonRequest request)
    {
        var session = _session.GetSession(request.SessionToken)
            ?? throw new InvalidOperationException("Lesson session not found or expired.");

        if (session.UserId != userId)
            throw new UnauthorizedAccessException("Session does not belong to this user.");

        _session.RemoveSession(request.SessionToken);

        var score = _scoring.CalculateScore(request.CorrectAnswers, request.TotalQuestions);
        var stars = _scoring.GetStars(score);

        var existing = await _progressRepo.GetByUserAndLessonAsync(userId, session.LessonId);
        var isFirstCompletion = existing is null;
        var isNewBest = existing is null || score > existing.BestScore;

        await _progressRepo.UpsertAsync(userId, session.LessonId, score, stars, isFirstCompletion, isNewBest);

        var coinsAwarded = 0;
        if (isFirstCompletion)
        {
            coinsAwarded = _scoring.BaseCoinsForStars(stars);
            await _coinService.AwardAsync(userId, coinsAwarded);
        }

        return new LessonCompleteResult(score, stars, isNewBest, isFirstCompletion, coinsAwarded);
    }

    private static string PillarDisplayName(Pillar pillar) => pillar switch
    {
        Pillar.SelfAwareness => "Self-Awareness",
        Pillar.SelfManagement => "Self-Management",
        Pillar.SocialAwareness => "Social Awareness",
        Pillar.RelationshipSkills => "Relationship Skills",
        Pillar.ResponsibleDecisionMaking => "Responsible Decision-Making",
        _ => pillar.ToString()
    };
}
