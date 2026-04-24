using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuckyEQ.Contracts.DTOs;
using DuckyEQ.Contracts.Models;
using DuckyEQ.Domain.Enums;

namespace DuckyEQ.Contracts.Interfaces.Services
{
    internal interface ILessonService
    {
        // Auto-creates missing PillarProgress rows on first call
        Task<IReadOnlyList<PillarProgressDto>> GetAllPillarProgressAsync(Guid userId);

        // Returns all 50 lessons for the pillar with per-lesson progress overlay
        Task<IReadOnlyList<LessonWithProgressDto>> GetLessonsForPillarAsync(
            Guid userId, Pillar pillar);

        Task<LessonContentDto> GetLessonContentAsync(Pillar pillar, int level);

        // Checks cooldown, creates session token, returns token + expiry
        Task<StartLessonResult> StartLessonAsync(Guid userId, Guid lessonId);

        // Validates session, scores, updates progress, awards coins
        Task<LessonCompleteResult> CompleteLessonAsync(
            Guid userId, CompleteLessonRequest request);

    }
}
