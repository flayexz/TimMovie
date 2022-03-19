using System.ComponentModel.DataAnnotations;

namespace TimMovie.Database.BaseEntities;

public abstract class PersonBaseEntity
{
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    
    [MaxLength(100)]
    public string? Surname { get; set; }
    public string? Photo { get; set; }
}