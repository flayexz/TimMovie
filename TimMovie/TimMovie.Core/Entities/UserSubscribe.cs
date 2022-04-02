using System.ComponentModel.DataAnnotations;

namespace TimMovie.Core.Entities;

public class UserSubscribe
{
    public Guid Id { get; set; }
    
    [Required]
    public User SubscribedUser { get; set; }
    
    [Required]
    public Subscribe Subscribe { get; set; }
    
    [Required]
    public DateTime StartDay { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
}