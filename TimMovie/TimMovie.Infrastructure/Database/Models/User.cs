using Microsoft.AspNetCore.Identity;

namespace TimMovie.Infrastructure.Database;

public class User : IdentityUser<Guid>
{
    public UserStatus Status { get; set; }
    public Film? WatchingFilm { get; set; }
    
    public Country? Country { get; set; }
    public DateTime RegistrationDate { get; set; }
    public List<UserFilmWatched> WatchedFilms { get; set; }
    public List<Notification> Notifications { get; set; }
    public List<Film> FilmsWatchLater { get; set; }
}