using System.ComponentModel.DataAnnotations;

namespace TimMovie.Core.Entities;

public class Banner
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    
    [Required]
    public string Image { get; set; }
    
    [Required]
    public Film Film { get; set; }
}