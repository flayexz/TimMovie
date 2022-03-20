using System.ComponentModel.DataAnnotations;

namespace TimMovie.Infrastructure.Database;

public class CommentReport
{
    public Guid Id { get; set; }
    
    [Required]
    public Comment Comment { get; set; }
    
    [Required]
    public User User { get; set; }
}