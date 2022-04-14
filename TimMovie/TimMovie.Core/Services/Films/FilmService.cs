using TimMovie.Core.Entities;
using TimMovie.Core.Specifications.InheritedSpecifications;
using TimMovie.Core.Specifications.StaticSpecification;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services.Films;

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
             .FirstOrDefault(new EntityByIdSpec<Film>(film.Id) 
                             && FilmStaticSpec.FilmIsIncludedAnySubscriptionSpec) is not null;
    }

    public double? GetRating(Film film)
    {
        return _filmRepository.Query
            .Where(new EntityByIdSpec<Film>(film.Id))
            .Select(f => f.UserFilmWatcheds.Select(watched => watched.Grade).Average())
            .FirstOrDefault();
    }
}