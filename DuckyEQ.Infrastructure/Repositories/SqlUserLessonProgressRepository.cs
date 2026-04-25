using DuckyEQ.Contracts.Interfaces.Repositories;
using DuckyEQ.Domain.Entities;
using DuckyEQ.Domain.Enums;
using DuckyEQ.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DuckyEQ.Infrastructure.Repositories;

public class SqlUserLessonProgressRepository : IUserLessonProgressRepository
{
    private readonly DuckyEQDbContext _db;
    public SqlUserLessonProgressRepository(DuckyEQDbContext db) => _db = db;

    public Task<UserLessonProgress?> GetByUserAndLessonAsync(Guid userId, Guid lessonId) =>
        _db.UserLessonProgresses.FirstOrDefaultAsync(p => p.UserId == userId && p.LessonId == lessonId);

    public async Task<IReadOnlyList<UserLessonProgress>> GetByUserAndPillarAsync(Guid userId, Pillar pillar)
    {
        var lessonIds = await _db.Lessons
            .Where(l => l.PillarId == pillar)
            .Select(l => l.Id)
            .ToListAsync();

        return await _db.UserLessonProgresses
            .Where(p => p.UserId == userId && lessonIds.Contains(p.LessonId))
            .ToListAsync();
    }

    public async Task UpsertAsync(Guid userId, Guid lessonId, int score, int stars, bool isFirstCompletion, bool isNewBest)
    {
        var existing = await _db.UserLessonProgresses
            .FirstOrDefaultAsync(p => p.UserId == userId && p.LessonId == lessonId);

        if (existing is null)
        {
            _db.UserLessonProgresses.Add(UserLessonProgress.Create(userId, lessonId, score, stars));
        }
        else
        {
            existing.RecordAttempt(score, stars);
        }

        await _db.SaveChangesAsync();
    }

    public async Task<DateTime?> GetLastNewLessonCompletedAtAsync(Guid userId, Pillar pillar)
    {
        var lessonIds = await _db.Lessons
            .Where(l => l.PillarId == pillar)
            .Select(l => l.Id)
            .ToListAsync();

        return await _db.UserLessonProgresses
            .Where(p => p.UserId == userId && lessonIds.Contains(p.LessonId) && p.FirstCompletedAt != null)
            .MaxAsync(p => (DateTime?)p.FirstCompletedAt);
    }
}
