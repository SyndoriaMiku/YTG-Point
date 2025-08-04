using YTG_Point.Data;
using YTG_Point.Models;
using Microsoft.EntityFrameworkCore;

namespace YTG_Point.Services;

public class RewardService
{
    private readonly ApplicationDbContext _context;

    public RewardService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Reward>> GetAllRewardsAsync()
    {
        return await _context.Rewards.OrderBy(r => r.RequiredPoints).ToListAsync();
    }

    public async Task<Reward> GetRewardByIdAsync(int rewardId)
    {
        return await _context.Rewards.FindAsync(rewardId);
    }

    public async Task<bool> CreateRewardAsync(string name, int requiredPoints)
    {
        var reward = new Reward
        {
            Name = name,
            RequiredPoints = requiredPoints
        };
        
        _context.Rewards.Add(reward);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateRewardAsync(int rewardId, string name, int requiredPoints)
    {
        var reward = await _context.Rewards.FindAsync(rewardId);
        if (reward == null)
            return false;
        
        reward.Name = name;
        reward.RequiredPoints = requiredPoints;
        
        _context.Rewards.Update(reward);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteRewardAsync(int rewardId)
    {
        var reward = await _context.Rewards.FindAsync(rewardId);
        if (reward == null)
            return false;
        
        _context.Rewards.Remove(reward);
        await _context.SaveChangesAsync();
        return true;
    }
}