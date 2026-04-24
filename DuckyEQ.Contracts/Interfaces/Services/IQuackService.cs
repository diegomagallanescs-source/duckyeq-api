using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuckyEQ.Contracts.DTOs;
using DuckyEQ.Contracts.Models;

namespace DuckyEQ.Contracts.Interfaces.Services
{
    internal interface IQuackService
    {
        // Throws QuackLimitExceededException (→ 429) if already sent today
        // Throws NotFriendsException (→ 403) if sender and recipient aren't friends
        Task<QuackDto> SendQuackAsync(Guid senderId, SendQuackRequest request);

        // Returns unseen quacks from last 48 hours for Home banner
        Task<IReadOnlyList<QuackDto>> GetUnseenAsync(Guid userId);

        // Validates that quackId belongs to userId before marking seen
        Task MarkSeenAsync(Guid userId, Guid quackId);

    }
}
