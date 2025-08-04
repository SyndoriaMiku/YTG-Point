
using System;
using System.Threading.Tasks;
using YTG_Point.Models;
using YTG_Point.Data;
using Microsoft.EntityFrameworkCore;

namespace YTG_Point.Services;

public class PointService
{
    private readonly ApplicationDbContext _context;
    
    public PointService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AdjustPointsAsync(string userId, int amount, string desctription)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return false;
        
        user.TotalPoints += amount;

        var transaction = new PointTransaction
        {
            UserId = userId,
            Amount = amount,
            Description = desctription,
            CreatedAt = DateTime.UtcNow
        };
        
        _context.PointTransactions.Add(transaction);
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<int> GetUserPointAsync(string userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return 0;
        return user.TotalPoints;
    }
    
}