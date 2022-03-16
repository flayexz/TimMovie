using System.ComponentModel.DataAnnotations;

namespace TimMovie.Database.Models;

public class Country
{
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }
}