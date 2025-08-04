using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YTG_Point.Models;
using YTG_Point.Services;

namespace YTG_Point.Controllers;

[Authorize]
public class RewardController : Controller
{ 
    private readonly RewardService _rewardService;
    private readonly TicketService _ticketService;

    public RewardController(RewardService rewardService, TicketService ticketService)
    {
        _rewardService = rewardService;
        _ticketService = ticketService;
    }

    public async Task<IActionResult> Index()
    {
        var reward = await _rewardService.GetAllRewardsAsync()
            return View(reward);
    }

    [HttpPost]
    public async Task<IActionResult> Redeem(int rewardId)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var success = await _ticketService.CreateTicketAsync(userId, rewardId);
        if (!success)
        {
            TempData["Error"] = "Cannot redemm reward";
        }
        else
        {
            TempData["Success"] = "You have successfully redeemed";
        }
        return RedirectToAction("Index");
        
    }
}