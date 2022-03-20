using System.ComponentModel.DataAnnotations;

namespace TimMovie.Infrastructure.Database;

public class Subscribe
{
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(70)]
    public string Name { get; set; }
    
    [Required]
    public float Price { get; set; }

    public string? Description { get; set; }
    
    public List<Film> Films { get; set; }
}