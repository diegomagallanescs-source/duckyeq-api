using System.Security.Claims;
using DuckyEQ.Contracts.Models;
using DuckyEQ.Services.Behaviors;
using DuckyEQ.Services.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DuckyEQ.Api.Controllers;

[ApiController]
[Route("api/checkin")]
[Authorize]
public class CheckInController : ControllerBase
{
    private readonly CheckInBehavior _checkIn;

    public CheckInController(CheckInBehavior checkIn)
    {
        _checkIn = checkIn;
    }

    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // 204 = user has not checked in today (React Native shows the modal)
    [HttpGet("today")]
    public async Task<IActionResult> GetToday()
    {
        var checkIn = await _checkIn.GetTodayAsync(CurrentUserId);
        if (checkIn is null)
            return NoContent();

        return Ok(checkIn);
    }

    [HttpPost]
    public async Task<IActionResult> CheckIn([FromBody] CheckInRequest request)
    {
        try
        {
            var result = await _checkIn.CheckInAsync(CurrentUserId, request);
            return StatusCode(201, result);
        }
        catch (AlreadyCheckedInException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
