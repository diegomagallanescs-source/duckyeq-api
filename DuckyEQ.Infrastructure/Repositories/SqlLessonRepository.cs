using DuckyEQ.Contracts.Interfaces.Repositories;
using DuckyEQ.Domain.Entities;
using DuckyEQ.Domain.Enums;
using DuckyEQ.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DuckyEQ.Infrastructure.Repositories;

public class SqlLessonRepository : ILessonRepository
{
    private readonly DuckyEQDbContext _db;
    public SqlLessonRepository(DuckyEQDbContext db) => _db = db;

    public async Task<IReadOnlyList<Lesson>> GetByPillarAsync(Pillar pillar) =>
        await _db.Lessons.Where(l => l.PillarId == pillar).OrderBy(l => l.Level).ToListAsync();

    public Task<Lesson?> GetByIdAsync(Guid id) =>
        _db.Lessons.FirstOrDefaultAsync(l => l.Id == id);

    public Task<Lesson?> GetByPillarAndLevelAsync(Pillar pillar, int level) =>
        _db.Lessons.FirstOrDefaultAsync(l => l.PillarId == pillar && l.Level == level);
}
