using System.Security.Claims;
using DuckyEQ.Contracts.Interfaces.Services;
using DuckyEQ.Contracts.Models;
using DuckyEQ.Services.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DuckyEQ.Api.Controllers;

[ApiController]
[Route("api/shop")]
[Authorize]
public class ShopController : ControllerBase
{
    private readonly IShopService _shop;

    public ShopController(IShopService shop)
    {
        _shop = shop;
    }

    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("items")]
    public async Task<IActionResult> GetItems()
    {
        var items = await _shop.GetActiveItemsAsync();
        return Ok(items);
    }

    [HttpPost("purchase")]
    public async Task<IActionResult> Purchase([FromBody] PurchaseRequest request)
    {
        try
        {
            var result = await _shop.PurchaseAsync(CurrentUserId, request.ShopItemId);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (AlreadyOwnedException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (InsufficientCoinsException ex)
        {
            return StatusCode(402, new { error = ex.Message });
        }
    }

    [HttpPost("equip")]
    public async Task<IActionResult> Equip([FromBody] EquipRequest request)
    {
        try
        {
            var equipped = await _shop.EquipAsync(CurrentUserId, request.ShopItemId);
            return Ok(equipped);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpGet("inventory")]
    public async Task<IActionResult> GetInventory()
    {
        var inventory = await _shop.GetInventoryAsync(CurrentUserId);
        return Ok(inventory);
    }
}
