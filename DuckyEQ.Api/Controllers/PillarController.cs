using System.Security.Claims;
using DuckyEQ.Domain.Enums;
using DuckyEQ.Services.Behaviors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DuckyEQ.Api.Controllers;

[ApiController]
[Route("api/pillars")]
[Authorize]
public class PillarController : ControllerBase
{
    private readonly LessonBehavior _lessons;

    public PillarController(LessonBehavior lessons)
    {
        _lessons = lessons;
    }

    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("progress")]
    public async Task<IActionResult> GetProgress()
    {
        var progress = await _lessons.GetAllPillarProgressAsync(CurrentUserId);
        return Ok(progress);
    }

    [HttpGet("{id}/lessons")]
    public async Task<IActionResult> GetLessons(int id)
    {
        if (!Enum.IsDefined(typeof(Pillar), id))
            return BadRequest(new { error = "Invalid pillar id." });

        var lessons = await _lessons.GetLessonsForPillarAsync(CurrentUserId, (Pillar)id);
        return Ok(lessons);
    }

    [HttpGet("{id}/cooldown-status")]
    public async Task<IActionResult> GetCooldownStatus(int id)
    {
        if (!Enum.IsDefined(typeof(Pillar), id))
            return BadRequest(new { error = "Invalid pillar id." });

        var status = await _lessons.GetCooldownStatusAsync(CurrentUserId, (Pillar)id);
        return Ok(status);
    }
}
