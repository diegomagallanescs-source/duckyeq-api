using DuckyEQ.Contracts.DTOs;
using DuckyEQ.Contracts.Interfaces.Repositories;
using DuckyEQ.Contracts.Interfaces.Services;
using DuckyEQ.Contracts.Models;
using DuckyEQ.Domain.Entities;

namespace DuckyEQ.Services.Services;

public class EQTestService : IEQTestService
{
    private readonly IEQTestQuestionRepository _questionRepo;
    private readonly IUserEQTestResultRepository _resultRepo;
    private readonly IEQTestScoringService _scoring;

    public EQTestService(
        IEQTestQuestionRepository questionRepo,
        IUserEQTestResultRepository resultRepo,
        IEQTestScoringService scoring)
    {
        _questionRepo = questionRepo;
        _resultRepo = resultRepo;
        _scoring = scoring;
    }

    public async Task<IReadOnlyList<EQTestQuestionDto>> GetQuestionsAsync()
    {
        var questions = await _questionRepo.GetRandomSetAsync(15);
        // CorrectOption intentionally excluded from DTO
        return questions.Select(q => new EQTestQuestionDto(
            q.Id, q.QuestionText, q.OptionA, q.OptionB, q.OptionC, q.OptionD)).ToList();
    }

    public async Task<EQTestResultDto> SubmitAsync(Guid userId, SubmitEQTestRequest request)
    {
        var allQuestions = await _questionRepo.GetAllAsync();
        var questionMap = allQuestions.ToDictionary(q => q.Id);

        var correct = request.Answers.Count(a =>
            questionMap.TryGetValue(a.QuestionId, out var q) &&
            a.SelectedOption == q.CorrectOption);

        var score = _scoring.CalculateScore(correct);
        var stars = _scoring.GetStars(score);

        var best = await _resultRepo.GetBestByUserAsync(userId);
        var isNewBest = best is null || score > best.Score;

        var result = UserEQTestResult.Create(userId, score, stars, correct);
        await _resultRepo.CreateAsync(result);

        return new EQTestResultDto(score, stars, correct, isNewBest);
    }

    public async Task<EQTestResultDto?> GetBestScoreAsync(Guid userId)
    {
        var best = await _resultRepo.GetBestByUserAsync(userId);
        if (best is null) return null;
        return new EQTestResultDto(best.Score, best.Stars, best.CorrectAnswers, false);
    }
}
