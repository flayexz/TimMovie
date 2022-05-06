using AutoMapper;
using TimMovie.Core.DTO.Films;
using TimMovie.Core.Entities;
using TimMovie.Core.Query;
using TimMovie.Core.Query.Films;
using TimMovie.Core.Specifications.InheritedSpecifications;
using TimMovie.Core.Specifications.InheritedSpecifications.FilmSpec;
using TimMovie.Core.Specifications.StaticSpecification;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services.Films;

public class FilmService
{
    private readonly IRepository<Film> _filmRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public FilmService(IRepository<Film> filmRepository, IRepository<User> userRepository, IMapper mapper)
    {
        _filmRepository = filmRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public bool IsExistInSubscribe(Film film)
    {
        return _filmRepository.Query
            .FirstOrDefault(new EntityByIdSpec<Film>(film.Id)
                            && FilmStaticSpec.FilmIsIncludedAnySubscriptionSpec) is not null;
    }

    public double? GetRating(Film film)
    {
        var rating = _filmRepository.Query
            .Where(new EntityByIdSpec<Film>(film.Id))
            .Select(f =>  f.UserFilmWatcheds.Select(watched => watched.Grade).Average())
            .FirstOrDefault();
        return rating.HasValue 
            ? Math.Round(rating.Value, 2)
            : null;
    }

    public IEnumerable<Film> GetFilmsByNamePart(string namePart, int count = int.MaxValue) =>
        _filmRepository.Query.Where(new FilmByNamePartSpec(namePart)).Take(count);

    public FilmForStatusDto? GetCurrentWatchingFilmByUser(Guid userId)
    {
        var query = _userRepository.Query
            .Where(new EntityByIdSpec<User>(userId));
        var executor = new QueryExecutor<User>(query, _userRepository);

        var film = executor
            .IncludeInResult(user => user.WatchingFilm)
            .FirstOrDefault();

        return MapToRequiredDto<User?, FilmForStatusDto>(film);
    }

    private TDto? MapToRequiredDto<T, TDto>(T entity)
        where TDto : class => entity is null
            ? null
            : _mapper.Map<TDto>(entity);

    public FilmDto? GetFilmById(Guid filmId)
    {
        var query = _filmRepository.Query
            .Where(new EntityByIdSpec<Film>(filmId));
        var executor = new QueryExecutor<Film>(query, _filmRepository);

        var film = executor
            .IncludeInResult(film => film.Genres)
            .IncludeInResult(film => film.Country)
            .IncludeInResult(film => film.Actors)
            .IncludeInResult(film => film.Producers)
            .IncludeInResult(film => film.Comments)
            .FirstOrDefault();
        
        return MapToRequiredDto<Film?, FilmDto>(film);
    }
}