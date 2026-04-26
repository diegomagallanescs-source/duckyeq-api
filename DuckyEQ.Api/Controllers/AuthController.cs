using DuckyEQ.Contracts.Models;
using DuckyEQ.Services.Behaviors;
using DuckyEQ.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DuckyEQ.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthBehavior _auth;

    public AuthController(AuthBehavior auth)
    {
        _auth = auth;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var result = await _auth.RegisterAsync(request);
            return CreatedAtAction(nameof(Register), result);
        }
        catch (EmailAlreadyExistsException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var result = await _auth.LoginAsync(request);
            return Ok(result);
        }
        catch (InvalidCredentialsException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }

    // Token arrives as a query param so it works from email verification links
    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromQuery] string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return BadRequest(new { error = "Verification token is required." });

        var success = await _auth.VerifyEmailAsync(token);
        if (!success)
            return BadRequest(new { error = "Invalid or expired verification token." });

        return Ok(new { message = "Email verified successfully." });
    }
}
