using Microsoft.AspNetCore.Identity;
using TimMovie.Database.Enums;

namespace TimMovie.Database.Models;

public class User : IdentityUser<int>
{
    public UserStatus Status { get; set; }
    public Film? WatchingFilm { get; set; }
    public Country? Country { get; set; }
    public DateTime RegistrationDate { get; set; }
    public List<UserFilmWatched> WatchedFilms { get; set; }
    public List<Notification> Notifications { get; set; }
    public List<Film> FilmsWatchLater { get; set; }
}