using System.ComponentModel.DataAnnotations;

namespace TimMovie.Database.Models;

public class Comment
{
    public Guid Id { get; set; }
    
    [Required]
    public Film Film { get; set; }
    
    [Required]
    public User Author { get; set; }

    [Required]
    public string Content { get; set; }
    
    [Required]
    public DateTime Date { get; set; }
}