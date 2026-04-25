using DuckyEQ.Contracts.Interfaces.Repositories;
using DuckyEQ.Domain.Entities;
using DuckyEQ.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DuckyEQ.Infrastructure.Repositories;

public class SqlQuackRepository : IQuackRepository
{
    private readonly DuckyEQDbContext _db;
    public SqlQuackRepository(DuckyEQDbContext db) => _db = db;

    public async Task<Quack> CreateAsync(Quack quack)
    {
        _db.Quacks.Add(quack);
        await _db.SaveChangesAsync();
        return quack;
    }

    // Unseen quacks sent within the last 48 hours
    public async Task<IReadOnlyList<Quack>> GetUnseenByRecipientAsync(Guid recipientId)
    {
        var cutoff = DateTime.UtcNow.AddHours(-48);
        return await _db.Quacks
            .Where(q => q.RecipientId == recipientId && q.SeenAt == null && q.SentAt >= cutoff)
            .ToListAsync();
    }

    public async Task MarkSeenAsync(Guid quackId)
    {
        var quack = await _db.Quacks.FirstOrDefaultAsync(q => q.Id == quackId);
        if (quack is not null)
        {
            quack.MarkSeen();
            await _db.SaveChangesAsync();
        }
    }

    // One Quack per (sender, recipient) pair per UTC calendar day
    public Task<bool> HasSentTodayAsync(Guid senderId, Guid recipientId)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        return _db.Quacks.AnyAsync(q =>
            q.SenderId == senderId &&
            q.RecipientId == recipientId &&
            DateOnly.FromDateTime(q.SentAt) == today);
    }
}
