using AutoMapper;
using TimMovie.Core.DTO.Films;
using TimMovie.Core.Entities;
using TimMovie.Core.Query;
using TimMovie.Core.Services.Actors;
using TimMovie.Core.Services.Producers;
using TimMovie.Core.Specifications.InheritedSpecifications;
using TimMovie.Core.Specifications.InheritedSpecifications.FilmSpec;
using TimMovie.Core.Specifications.StaticSpecification;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services.Films;

public class FilmService
{
    private readonly IRepository<Film> _filmRepository;
    private readonly IRepository<User> _userRepository;
    private readonly ProducerService _producerService;
    private readonly ActorService _actorService;
    private readonly IMapper _mapper;

    public FilmService(
        IRepository<Film> filmRepository,
        IRepository<User> userRepository,
        IMapper mapper,
        ProducerService producerService,
        ActorService actorService)
    {
        _filmRepository = filmRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _producerService = producerService;
        _actorService = actorService;
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
            .Select(f => f.UserFilmWatcheds.Select(watched => watched.Grade).Average())
            .FirstOrDefault();
        return rating.HasValue
            ? Math.Round(rating.Value, 1)
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

    private int? GetGradesAmount(Guid filmId)
    {
        var query = _filmRepository.Query
            .Where(new EntityByIdSpec<Film>(filmId));
        var executor = new QueryExecutor<Film>(query, _filmRepository);
    
        var film = executor
            .IncludeInResult(film => film.UserFilmWatcheds).FirstOrDefault();
        return film?.UserFilmWatcheds.Count;
    }
    
    public FilmDto? GetFilmById(Guid filmId)
    {
        var t = GetGradesAmount(filmId);
        var query = _filmRepository.Query
            .Where(new EntityByIdSpec<Film>(filmId));
        var executor = new QueryExecutor<Film>(query, _filmRepository);

        var tmpFilm = executor
            .IncludeInResult(film => film.Genres)
            .IncludeInResult(film => film.Country)
            .IncludeInResult(film => film.Actors)
            .IncludeInResult(film => film.Producers)
            .IncludeInResult(film => film.Comments)
            .IncludeInResult(film => film.UserFilmWatcheds)
            .FirstOrDefault();


        var film = MapToRequiredDto<Film?, FilmDto>(tmpFilm);
        film!.Rating = GetRating(tmpFilm!);
        film!.GradesNumber = tmpFilm!.UserFilmWatcheds.Count; 
        return film;
    }
}