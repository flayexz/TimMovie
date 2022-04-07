﻿using System.ComponentModel.DataAnnotations;
using TimMovie.Core.Entities.Enums;
using TimMovie.SharedKernel.BaseEntities;
using TimMovie.SharedKernel.Classes;

namespace TimMovie.Core.Entities;

public class User : UserBaseEntity
{
    [DateOnly] public DateOnly BirthDate { get; set; }
    public UserStatus Status { get; set; }
    public Film? WatchingFilm { get; set; }

    public Country? Country { get; set; }
    public DateTime RegistrationDate { get; set; }
    public List<UserFilmWatched> WatchedFilms { get; set; }
    public List<Notification> Notifications { get; set; }
    public List<Film> FilmsWatchLater { get; set; }
}