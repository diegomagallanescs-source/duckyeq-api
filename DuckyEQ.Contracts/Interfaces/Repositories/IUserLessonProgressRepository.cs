using DuckyEQ.Domain.Entities;
using DuckyEQ.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckyEQ.Contracts.Interfaces.Repositories
{
    internal interface IUserLessonProgressRepository
    {
        Task<UserLessonProgress?> GetByUserAndLessonAsync(Guid userId, Guid lessonId);
        Task<IReadOnlyList<UserLessonProgress>> GetByUserAndPillarAsync(
            Guid userId, Pillar pillar);
        Task UpsertAsync(Guid userId, Guid lessonId, int score,
            int stars, bool isFirstCompletion, bool isNewBest);
        Task<DateTime?> GetLastNewLessonCompletedAtAsync(Guid userId, Pillar pillar);

    }
}
