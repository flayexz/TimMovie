using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services;

public class FilmService
{
    private readonly IRepository<Genre> _genreRepository;
    private readonly IRepository<Country> _countryRepository;
    private readonly IRepository<Film> _filmRepository;

    public FilmService(IRepository<Genre> genreRepository, 
        IRepository<Country> countryRepository,
        IRepository<Film> filmRepository)
    {
        _genreRepository = genreRepository;
        _countryRepository = countryRepository;
        _filmRepository = filmRepository;
    }

    public IEnumerable<string> GetGenreNames()
    {
        return _genreRepository.Query.Select(genre => genre.Name).ToList();
    }

    public IEnumerable<string> GetCountryName()
    {
        return _countryRepository.Query.Select(country => country.Name).ToList();
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