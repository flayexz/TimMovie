using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services;

public class FilmService
{
    private readonly IRepository<Film> _filmRepository;

    public FilmService(IRepository<Film> filmRepository)
    {
        _filmRepository = filmRepository;
    }

    public bool IsExistInSubscribe(Film film)
    {
         return _filmRepository.Query
             .FirstOrDefault(f => film.Id == f.Id && f.Subscribes.Any()) is not null;
    }

    public double? GetRating(Film film)
    {
        return _filmRepository.Query
            .Where(f => f.Id == film.Id)
            .Select(f => f.UserFilmWatcheds.Select(watched => watched.Grade).Average())
            .FirstOrDefault();
    }
}