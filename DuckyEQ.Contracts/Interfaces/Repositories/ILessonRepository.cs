using DuckyEQ.Domain.Entities;
using DuckyEQ.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.Interfaces.Repositories
{
    public interface ILessonRepository
    {
        Task<IReadOnlyList<Lesson>> GetByPillarAsync(Pillar pillar);
        Task<Lesson?> GetByIdAsync(Guid id);
        Task<Lesson?> GetByPillarAndLevelAsync(Pillar pillar, int level);

    }
}

