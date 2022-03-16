using System.ComponentModel.DataAnnotations;

namespace TimMovie.Database.Models;

public class CommentReport
{
    public Guid Id { get; set; }
    
    [Required]
    public Comment Comment { get; set; }
    
    [Required]
    public User User { get; set; }
}