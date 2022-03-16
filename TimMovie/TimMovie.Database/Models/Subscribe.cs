using System.ComponentModel.DataAnnotations;

namespace TimMovie.Database.Models;

public class Subscribe
{
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public float Price { get; set; }
    
    public List<Film> Films { get; set; }
}