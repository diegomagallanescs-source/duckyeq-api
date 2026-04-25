using DuckyEQ.Contracts.Interfaces.Repositories;
using DuckyEQ.Domain.Entities;
using DuckyEQ.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DuckyEQ.Infrastructure.Repositories;

public class SqlEQTestQuestionRepository : IEQTestQuestionRepository
{
    private readonly DuckyEQDbContext _db;
    public SqlEQTestQuestionRepository(DuckyEQDbContext db) => _db = db;

    public async Task<IReadOnlyList<EQTestQuestion>> GetRandomSetAsync(int count) =>
        await _db.EQTestQuestions.OrderBy(_ => Guid.NewGuid()).Take(count).ToListAsync();

    public async Task<IReadOnlyList<EQTestQuestion>> GetAllAsync() =>
        await _db.EQTestQuestions.ToListAsync();
}
