using AutoMapper;
using TimMovie.Core.DTO.Films;
using TimMovie.Core.Entities;
using TimMovie.Core.Query;
using TimMovie.Core.Services.WatchedFilms;
using TimMovie.Core.Specifications.InheritedSpecifications;
using TimMovie.Core.Specifications.InheritedSpecifications.FilmSpec;
using TimMovie.Core.Specifications.StaticSpecification;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services.Films;

public class FilmService
{
    private readonly IRepository<Film> _filmRepository;
    private readonly IRepository<User> _userRepository;
    private readonly Lazy<WatchedFilmService> _watchedFilmService;
    private readonly IMapper _mapper;

    public FilmService(
        IRepository<Film> filmRepository,
        IRepository<User> userRepository,
        IMapper mapper,
        Lazy<WatchedFilmService> watchedFilmService)
    {
        _filmRepository = filmRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _watchedFilmService = watchedFilmService;
    }

    public bool TryGetUserGrade(Guid filmId, Guid userId, out int? grade)
    {
        grade = null;
        var dbFilm = _filmRepository.Query.FirstOrDefault(new EntityByIdSpec<Film>(filmId));
        if (dbFilm is null)
            return false;

        var user = _userRepository.Query.FirstOrDefault(new EntityByIdSpec<User>(userId));
        if (user is null)
            return false;

        grade = _filmRepository.Query
            .Where(new EntityByIdSpec<Film>(filmId))
            .Select(f =>
                f.UserFilmWatcheds
                    .Where(watched => watched.WatchedUser == user)
                    .Select(watched => watched.Grade)
                    .FirstOrDefault())
            .FirstOrDefault();
        return true;
    }

    public async Task<bool> TryUpdateFilmGrade(Guid filmId, Guid userId, int grade)
    {
        var dbFilm = _filmRepository.Query.FirstOrDefault(new EntityByIdSpec<Film>(filmId));
        if (dbFilm is null)
            return false;

        var userQuery = _userRepository.Query.Where(new EntityByIdSpec<User>(userId));
        var userQueryExecutor = new QueryExecutor<User>(userQuery, _userRepository);
        var user = userQueryExecutor
            .IncludeInResult(user => user.WatchedFilms)
            .FirstOrDefault();
        if (user is null)
            return false;

        // ReSharper disable once ConstantNullCoalescingCondition
        user.WatchedFilms ??= new List<UserFilmWatched>();
        var watchedFilms = user.WatchedFilms.FirstOrDefault(watched => watched.Film == dbFilm);
        if (watchedFilms is null)
            user.WatchedFilms.Add(new UserFilmWatched
            {
                Date = DateTime.Now,
                Film = dbFilm,
                Grade = grade,
                WatchedUser = user
            });
        else
            watchedFilms.Grade = grade;
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();
        return true;
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

    public FilmDto? GetFilmById(Guid filmId)
    {
        var dbFilm = GetDbFilmById(filmId);
        var film = MapToRequiredDto<Film?, FilmDto>(dbFilm);
        film!.Rating = GetRating(dbFilm!);
        film!.GradesNumber = _watchedFilmService.Value.GetAmountGradesForFilms(filmId);
        return film;
    }

    public Film? GetDbFilmById(Guid filmId)
    {
        var query = _filmRepository.Query
            .Where(new EntityByIdSpec<Film>(filmId));
        var executor = new QueryExecutor<Film>(query, _filmRepository);

        var tmpFilm = executor
            .IncludeInResult(film => film.Genres)
            .IncludeInResult(film => film.Country)
            .IncludeInResult(film => film.Actors)
            .IncludeInResult(film => film.Producers)
            .IncludeInResult(film => film.Comments)
            .FirstOrDefault();
        return tmpFilm;
    }
}