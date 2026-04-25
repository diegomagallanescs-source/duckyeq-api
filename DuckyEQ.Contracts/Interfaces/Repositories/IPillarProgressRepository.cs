using DuckyEQ.Domain.Entities;
using DuckyEQ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.Interfaces.Repositories
{
    public interface IPillarProgressRepository
    {
        Task<PillarProgress?> GetByUserAndPillarAsync(Guid userId, Pillar pillar);
        Task<IReadOnlyList<PillarProgress>> GetAllByUserAsync(Guid userId);
        Task<PillarProgress> CreateAsync(PillarProgress progress);
        Task UpdateAsync(PillarProgress progress);
        Task EnsureAllPillarsExistAsync(Guid userId);

    }
}

