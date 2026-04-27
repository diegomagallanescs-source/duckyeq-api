using System.Security.Claims;
using DuckyEQ.Contracts.Interfaces.Services;
using DuckyEQ.Contracts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DuckyEQ.Api.Controllers;

[ApiController]
[Route("api/eq-test")]
[Authorize]
public class EQTestController : ControllerBase
{
    private readonly IEQTestService _eqTest;

    public EQTestController(IEQTestService eqTest)
    {
        _eqTest = eqTest;
    }

    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("questions")]
    public async Task<IActionResult> GetQuestions()
    {
        var questions = await _eqTest.GetQuestionsAsync();
        return Ok(questions);
    }

    [HttpPost("submit")]
    public async Task<IActionResult> Submit([FromBody] SubmitEQTestRequest request)
    {
        var result = await _eqTest.SubmitAsync(CurrentUserId, request);
        return Ok(result);
    }

    // Returns 204 if the user has never taken the test
    [HttpGet("best-score")]
    public async Task<IActionResult> GetBestScore()
    {
        var best = await _eqTest.GetBestScoreAsync(CurrentUserId);
        if (best is null)
            return NoContent();

        return Ok(best);
    }
}
