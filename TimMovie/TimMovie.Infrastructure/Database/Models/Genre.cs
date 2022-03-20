using System.ComponentModel.DataAnnotations;

namespace TimMovie.Infrastructure.Database;

public class Genre
{
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    public List<Film> Films { get; set; }
}