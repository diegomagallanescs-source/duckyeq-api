using DuckyEQ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.Interfaces.Repositories
{
    public interface IQuackRepository
    {
        Task<Quack> CreateAsync(Quack quack);

        // Returns quacks WHERE RecipientId = userId AND SeenAt IS NULL
        // Filtered to last 48 hours to keep banner data fresh
        Task<IReadOnlyList<Quack>> GetUnseenByRecipientAsync(Guid recipientId);

        // Sets SeenAt = UTC_NOW for the given quack
        Task MarkSeenAsync(Guid quackId);

        // Checks if sender already sent a quack to recipient today (calendar day UTC)
        // Used by service layer to enforce one-per-day rate limit
        Task<bool> HasSentTodayAsync(Guid senderId, Guid recipientId);

    }
}

