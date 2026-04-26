using System.Security.Claims;
using DuckyEQ.Contracts.Models;
using DuckyEQ.Domain.Enums;
using DuckyEQ.Services.Behaviors;
using DuckyEQ.Services.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DuckyEQ.Api.Controllers;

[ApiController]
[Route("api/lessons")]
[Authorize]
public class LessonController : ControllerBase
{
    private readonly LessonBehavior _lessons;

    public LessonController(LessonBehavior lessons)
    {
        _lessons = lessons;
    }

    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("{pillarId}/{level}")]
    public async Task<IActionResult> GetLessonContent(int pillarId, int level)
    {
        if (!Enum.IsDefined(typeof(Pillar), pillarId))
            return BadRequest(new { error = "Invalid pillar id." });

        try
        {
            var content = await _lessons.GetLessonContentAsync((Pillar)pillarId, level);
            return Ok(content);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpPost("{id}/start")]
    public async Task<IActionResult> StartLesson(Guid id)
    {
        try
        {
            var result = await _lessons.StartLessonAsync(CurrentUserId, id);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (CooldownActiveException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpPost("{id}/complete")]
    public async Task<IActionResult> CompleteLesson(Guid id, [FromBody] CompleteLessonRequest request)
    {
        try
        {
            var result = await _lessons.CompleteLessonAsync(CurrentUserId, request);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}
