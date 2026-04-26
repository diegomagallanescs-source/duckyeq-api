using System.Security.Claims;
using DuckyEQ.Contracts.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DuckyEQ.Api.Controllers;

[ApiController]
[Route("api/coins")]
[Authorize]
public class CoinController : ControllerBase
{
    private readonly ICoinService _coins;

    public CoinController(ICoinService coins)
    {
        _coins = coins;
    }

    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("balance")]
    public async Task<IActionResult> GetBalance()
    {
        var balance = await _coins.GetBalanceAsync(CurrentUserId);
        return Ok(new { balance });
    }
}
