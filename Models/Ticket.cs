using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YTG_Point.Models;

public class Ticket
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string UserId { get; set; }
    [ForeignKey("UserID")]
    public AppUser User { get; set; }
    
    [Required]
    public int RewardId { get; set; }
    [ForeignKey("RewardId")]
    public Reward Reward { get; set; }

    [Required] public TicketStatus Status { get; set; } = TicketStatus.Pending;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessAt { get; set; } = null;
}

public enum TicketStatus
{
    Pending,
    Approved,
    Rejected,
}
