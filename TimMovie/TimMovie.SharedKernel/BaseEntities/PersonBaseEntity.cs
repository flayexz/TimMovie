using System.ComponentModel.DataAnnotations;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.SharedKernel.BaseEntities;

public abstract class PersonBaseEntity : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    
    [MaxLength(100)]
    public string? Surname { get; set; }
    public string? Photo { get; set; }
}