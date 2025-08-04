using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YTG_Point.Models;

namespace YTG_Point.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public DbSet<Reward> Rewards { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<PointTransaction> PointTransactions { get; set; }
    
}