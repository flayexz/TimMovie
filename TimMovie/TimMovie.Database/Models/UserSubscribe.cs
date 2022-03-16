namespace TimMovie.Database.Models;

public class UserSubscribe
{
    public Guid Id { get; set; }
    
    public User SubscribedUser { get; set; }
    
    public Subscribe Subscribe { get; set; }
    
    public DateTime StartDay { get; set; }
    
    public DateTime EndDate { get; set; }
}