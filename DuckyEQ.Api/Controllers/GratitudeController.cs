using System.Security.Claims;
using DuckyEQ.Contracts.Interfaces.Services;
using DuckyEQ.Contracts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DuckyEQ.Api.Controllers;

[ApiController]
[Route("api/gratitude")]
[Authorize]
public class GratitudeController : ControllerBase
{
    private readonly IGratitudeService _gratitude;

    public GratitudeController(IGratitudeService gratitude)
    {
        _gratitude = gratitude;
    }

    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public async Task<IActionResult> AddEntry([FromBody] AddGratitudeRequest request)
    {
        var result = await _gratitude.AddEntryAsync(CurrentUserId, request);
        return StatusCode(201, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var entries = await _gratitude.GetAllPagedAsync(CurrentUserId, page, pageSize);
        return Ok(entries);
    }

    // Returns 204 when < 3 entries exist (Pick Me Up guard — not enough history)
    [HttpGet("random")]
    public async Task<IActionResult> GetRandom()
    {
        var entry = await _gratitude.GetRandomAsync(CurrentUserId);
        if (entry is null)
            return NoContent();

        return Ok(entry);
    }

    [HttpGet("streak")]
    public async Task<IActionResult> GetStreak()
    {
        var streak = await _gratitude.GetStreakAsync(CurrentUserId);
        return Ok(streak);
    }
}
