using System.ComponentModel.DataAnnotations;

namespace YTG_Point.Models;

public class Reward
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Name { get; set; }
    
    [Required]
    public int RequiredPoints { get; set; }
    
    public string Description { get; set; }
    
    public ICollection<Ticket>  Tickets { get; set; }
}