using System.ComponentModel.DataAnnotations;

namespace TimMovie.Core.Entities;

public class UserFilmWatched
{
    public Guid Id { get; set; }
    
    [Required]
    public User WatchedUser { get; set; }
    
    [Required]
    public Film Film { get; set; }
    
    [Range(1,10)]
    public int? Grade { get; set; }
    
    [Required]
    public DateTime Date { get; set; }
}