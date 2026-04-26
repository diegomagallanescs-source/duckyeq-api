using System.Security.Claims;
using DuckyEQ.Contracts.Models;
using DuckyEQ.Services.Behaviors;
using DuckyEQ.Services.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DuckyEQ.Api.Controllers;

[ApiController]
[Route("api/user")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly AuthBehavior _auth;

    public UserController(AuthBehavior auth)
    {
        _auth = auth;
    }

    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            var profile = await _auth.GetProfileAsync(CurrentUserId);
            return Ok(profile);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpPut("duck-character")]
    public async Task<IActionResult> UpdateDuckCharacter([FromBody] UpdateDuckCharacterRequest request)
    {
        try
        {
            await _auth.UpdateDuckCharacterAsync(CurrentUserId, request.DuckCharacter);
            return Ok(new { message = "Duck character updated." });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpPut("known-as")]
    public async Task<IActionResult> UpdateKnownAs([FromBody] UpdateKnownAsRequest request)
    {
        try
        {
            // Reissues a full JWT so the client immediately has the updated KnownAs claim
            var result = await _auth.UpdateKnownAsAsync(CurrentUserId, request.KnownAs);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}
