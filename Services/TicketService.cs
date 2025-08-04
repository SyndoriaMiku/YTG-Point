
using Microsoft.EntityFrameworkCore;
using YTG_Point.Data;
using YTG_Point.Models;

namespace YTG_Point.Services;

public class TicketService
{
    private readonly ApplicationDbContext _context;

    public TicketService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateTicketAsync(string userId, int rewardId)
    {
        var reward = await _context.Rewards.FindAsync(rewardId);
        if (reward == null)
            return false;
        
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return false;

        var ticket = new Ticket
        {
            UserId = userId,
            RewardId = rewardId,
            Status = TicketStatus.Pending,
            CreatedAt = DateTime.UtcNow,
        };
        
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Ticket>> GetUserTicketsAsync(string userId)
    {
        return await _context.Tickets
            .Include(t => t.Reward)
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
        
    }

    public async Task<List<Ticket>> GetPendingTicketAsync()
    {
        return await _context.Tickets
            .Include(t => t.User)
            .Include(t => t.Status == TicketStatus.Pending)
            .OrderBy(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> ApproveTicketAsync(int ticketId)
    {
        var ticket = await _context.Tickets.Include(t => t.User).Include(t => t.Reward)
            .FirstOrDefaultAsync(t => t.Id == ticketId);
        if (ticket == null || ticket.Status != TicketStatus.Pending)
            return false;
        var user = ticket.User;
        var reward = ticket.Reward;
        
        if (user.TotalPoints < reward.RequiredPoints)
            return false;
        
        user.TotalPoints -= reward.RequiredPoints;

        var transaction = new PointTransaction
        {
            UserId = user.Id,
            Amount = -reward.RequiredPoints,
            Description = $"Redeemed",
            CreatedAt = DateTime.UtcNow,
        };
        
        ticket.Status = TicketStatus.Approved;
        ticket.ProcessAt = DateTime.UtcNow;
        
        _context.PointTransactions.Add(transaction);
        _context.Tickets.Update(ticket);
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RejectTicketAsync(int ticketId)
    {
        var ticket = await _context.Tickets.FindAsync(ticketId);
        if (ticket == null || ticket.Status != TicketStatus.Pending)
            return false;
        
        ticket.Status = TicketStatus.Rejected;
        ticket.ProcessAt = DateTime.UtcNow;
        
        _context.Tickets.Update(ticket);
        await _context.SaveChangesAsync();
        return true;
    }
}