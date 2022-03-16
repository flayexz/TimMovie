using System.ComponentModel.DataAnnotations;

namespace TimMovie.Database.Models;

public class UserFilmWatched
{
    public Guid Id { get; set; }
    
    public User WatchedUser { get; set; }
    
    public Film Film { get; set; }
    
    [Range(1,10)]
    public int? Grade { get; set; }
    
    public DateTime Date { get; set; }
}