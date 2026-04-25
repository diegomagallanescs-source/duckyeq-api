using DuckyEQ.Contracts.Interfaces.Repositories;
using DuckyEQ.Domain.Entities;
using DuckyEQ.Domain.Enums;
using DuckyEQ.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DuckyEQ.Infrastructure.Repositories;

public class SqlPillarProgressRepository : IPillarProgressRepository
{
    private readonly DuckyEQDbContext _db;
    public SqlPillarProgressRepository(DuckyEQDbContext db) => _db = db;

    public Task<PillarProgress?> GetByUserAndPillarAsync(Guid userId, Pillar pillar) =>
        _db.PillarProgresses.FirstOrDefaultAsync(p => p.UserId == userId && p.Pillar == pillar);

    public async Task<IReadOnlyList<PillarProgress>> GetAllByUserAsync(Guid userId) =>
        await _db.PillarProgresses.Where(p => p.UserId == userId).ToListAsync();

    public async Task<PillarProgress> CreateAsync(PillarProgress progress)
    {
        _db.PillarProgresses.Add(progress);
        await _db.SaveChangesAsync();
        return progress;
    }

    public async Task UpdateAsync(PillarProgress progress)
    {
        _db.PillarProgresses.Update(progress);
        await _db.SaveChangesAsync();
    }

    public async Task EnsureAllPillarsExistAsync(Guid userId)
    {
        var existing = await _db.PillarProgresses
            .Where(p => p.UserId == userId)
            .Select(p => p.Pillar)
            .ToListAsync();

        var missing = Enum.GetValues<Pillar>()
            .Where(p => !existing.Contains(p))
            .Select(p => PillarProgress.Create(userId, p))
            .ToList();

        if (missing.Count > 0)
        {
            _db.PillarProgresses.AddRange(missing);
            await _db.SaveChangesAsync();
        }
    }
}
