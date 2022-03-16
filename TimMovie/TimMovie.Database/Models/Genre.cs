using System.ComponentModel.DataAnnotations;

namespace TimMovie.Database.Models;

public class Genre
{
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public List<Film> Films { get; set; }
}