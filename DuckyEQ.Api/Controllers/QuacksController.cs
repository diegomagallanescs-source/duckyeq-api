using System.Security.Claims;
using DuckyEQ.Contracts.Models;
using DuckyEQ.Services.Behaviors;
using DuckyEQ.Services.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DuckyEQ.Api.Controllers;

[ApiController]
[Route("api/quacks")]
[Authorize]
public class QuacksController : ControllerBase
{
    private readonly QuackBehavior _quacks;

    public QuacksController(QuackBehavior quacks)
    {
        _quacks = quacks;
    }

    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public async Task<IActionResult> SendQuack([FromBody] SendQuackRequest request)
    {
        try
        {
            var quack = await _quacks.SendQuackAsync(CurrentUserId, request);
            return StatusCode(201, quack);
        }
        catch (NotFriendsException ex)
        {
            return StatusCode(403, new { error = ex.Message });
        }
        catch (QuackLimitExceededException ex)
        {
            var midnightUtc = DateTime.UtcNow.Date.AddDays(1);
            Response.Headers["Retry-After"] = midnightUtc.ToString("R");
            return StatusCode(429, new { error = ex.Message });
        }
    }

    [HttpGet("unseen")]
    public async Task<IActionResult> GetUnseen()
    {
        var quacks = await _quacks.GetUnseenAsync(CurrentUserId);
        return Ok(quacks);
    }

    [HttpPatch("{id}/seen")]
    public async Task<IActionResult> MarkSeen(Guid id)
    {
        try
        {
            await _quacks.MarkSeenAsync(CurrentUserId, id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}
