using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YTG_Point.Models;

public class PointTransaction
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string UserId { get; set; }
    
    [ForeignKey("UserId")]
    public AppUser User { get; set; }
    
    [Required]
    public int Amount { get; set; } // Deduce point if negative
    
    [MaxLength(255)]
    public string Description { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}