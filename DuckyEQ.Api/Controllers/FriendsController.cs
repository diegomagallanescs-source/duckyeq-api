using System.Security.Claims;
using DuckyEQ.Contracts.Models;
using DuckyEQ.Services.Behaviors;
using DuckyEQ.Services.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DuckyEQ.Api.Controllers;

[ApiController]
[Route("api")]
[Authorize]
public class FriendsController : ControllerBase
{
    private readonly FriendsBehavior _friends;

    public FriendsController(FriendsBehavior friends)
    {
        _friends = friends;
    }

    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("users/search")]
    public async Task<IActionResult> SearchUsers([FromQuery] string username)
    {
        if (string.IsNullOrWhiteSpace(username) || username.Length < 3)
            return BadRequest(new { error = "Search query must be at least 3 characters." });

        var results = await _friends.SearchUsersAsync(CurrentUserId, username);
        return Ok(results);
    }

    [HttpPost("friends/request")]
    public async Task<IActionResult> SendRequest([FromBody] SendFriendRequestPayload payload)
    {
        try
        {
            var friendship = await _friends.SendRequestAsync(CurrentUserId, payload.TargetUsername);
            return StatusCode(201, friendship);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (FriendRequestConflictException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpGet("friends/requests/pending")]
    public async Task<IActionResult> GetPendingRequests()
    {
        var requests = await _friends.GetPendingIncomingAsync(CurrentUserId);
        return Ok(requests);
    }

    [HttpPut("friends/requests/{id}/accept")]
    public async Task<IActionResult> AcceptRequest(Guid id)
    {
        try
        {
            var friendship = await _friends.AcceptRequestAsync(CurrentUserId, id);
            return Ok(friendship);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpPut("friends/requests/{id}/decline")]
    public async Task<IActionResult> DeclineRequest(Guid id)
    {
        try
        {
            await _friends.DeclineRequestAsync(CurrentUserId, id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpGet("friends")]
    public async Task<IActionResult> GetFriends()
    {
        var friends = await _friends.GetFriendsWithCheckInAsync(CurrentUserId);
        return Ok(friends);
    }

    // {id} is the friend's userId, not the friendshipId
    [HttpGet("friends/{id}")]
    public async Task<IActionResult> GetFriendDetail(Guid id)
    {
        try
        {
            var detail = await _friends.GetFriendDetailAsync(CurrentUserId, id);
            return Ok(detail);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}
