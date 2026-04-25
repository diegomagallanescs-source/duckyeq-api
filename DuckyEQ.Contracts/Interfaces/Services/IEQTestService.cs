using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuckyEQ.Contracts.DTOs;
using DuckyEQ.Contracts.Models;

namespace DuckyEQ.Contracts.Interfaces.Services
{
    public interface IEQTestService
    {
        // Returns 15 random questions — CorrectOption stripped from DTO
        Task<IReadOnlyList<EQTestQuestionDto>> GetQuestionsAsync();

        // Scores submission, persists UserEQTestResult, returns result
        Task<EQTestResultDto> SubmitAsync(
            Guid userId, SubmitEQTestRequest request);

        // Returns null if user has never taken the test
        Task<EQTestResultDto?> GetBestScoreAsync(Guid userId);

    }
}

