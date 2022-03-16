using System.ComponentModel.DataAnnotations;

namespace TimMovie.Database.Models;

public class Notification
{
    public Guid Id { get; set; }
    
    [Required]
    public string Content { get; set; }
    
    public List<User> Users { get; set; }

}