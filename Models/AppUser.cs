using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace YTG_Point.Models;

public class AppUser : IdentityUser
{
    public int TotalPoints { get; set; } = 0;
    
    public ICollection<Ticket> Tickets { get; set; }
    public ICollection<PointTransaction> PointTransactions { get; set; }
}