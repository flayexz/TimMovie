using System.ComponentModel.DataAnnotations;

namespace TimMovie.Core.Entities;

public class CommentReport
{
    public Guid Id { get; set; }
    
    [Required]
    public Comment Comment { get; set; }
    
    [Required]
    public User User { get; set; }
}