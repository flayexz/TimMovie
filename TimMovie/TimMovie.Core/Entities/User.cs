using TimMovie.Core.Entities.Enums;
using TimMovie.SharedKernel.BaseEntities;

namespace TimMovie.Core.Entities;

public class User : UserBaseEntity
{
    public UserStatus Status { get; set; }
    public Film? WatchingFilm { get; set; }
    
    public Country? Country { get; set; }
    public DateTime RegistrationDate { get; set; }
    public List<UserFilmWatched> WatchedFilms { get; set; }
    public List<Notification> Notifications { get; set; }
    public List<Film> FilmsWatchLater { get; set; }
}