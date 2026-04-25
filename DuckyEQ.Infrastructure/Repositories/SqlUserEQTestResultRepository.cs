using DuckyEQ.Contracts.Interfaces.Repositories;
using DuckyEQ.Domain.Entities;
using DuckyEQ.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DuckyEQ.Infrastructure.Repositories;

public class SqlUserEQTestResultRepository : IUserEQTestResultRepository
{
    private readonly DuckyEQDbContext _db;
    public SqlUserEQTestResultRepository(DuckyEQDbContext db) => _db = db;

    public async Task<UserEQTestResult> CreateAsync(UserEQTestResult result)
    {
        _db.UserEQTestResults.Add(result);
        await _db.SaveChangesAsync();
        return result;
    }

    public Task<UserEQTestResult?> GetBestByUserAsync(Guid userId) =>
        _db.UserEQTestResults
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.Score)
            .FirstOrDefaultAsync();
}
