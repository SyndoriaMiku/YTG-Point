using YTG_Point.Data;
using YTG_Point.Models;
using Microsoft.EntityFrameworkCore;

namespace YTG_Point.Services;

public class PointTransactionService
{
        private readonly ApplicationDbContext _context;

        public PointTransactionService(ApplicationDbContext context)
        { 
            _context = context;
        }

        public async Task<List<PointTransaction>> GetUserTransactionAsync(string userId)
        {
            return await _context.PointTransactions
                .Where(t => t.UserId == userId)
                .OrderByDescending((t => t.CreatedAt))
                .ToListAsync();
        }

        public async Task<bool> AddPointAsync(string userId, int amount, string description)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            var transaction = new PointTransaction
            {
                UserId = userId,
                Amount = amount,
                Description = description,
                CreatedAt = DateTime.Now,
            };
            
            user.TotalPoints += amount;
            _context.PointTransactions.Add(transaction);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeductPointAsync(string userId, int amount, string description)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || amount == 0 || user.TotalPoints < amount)
                return false;

            var transaction = new PointTransaction
            {
                UserId = userId,
                Amount = -amount,
                Description = description,
                CreatedAt = DateTime.Now,
            };
            
            user.TotalPoints -= amount;
            _context.PointTransactions.Add(transaction);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
        
}