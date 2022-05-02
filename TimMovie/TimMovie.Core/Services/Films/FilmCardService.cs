using AutoMapper;
using TimMovie.Core.DTO;
using TimMovie.Core.DTO.Films;
using TimMovie.Core.Entities;
using TimMovie.Core.Query;
using TimMovie.Core.Query.Films;
using TimMovie.SharedKernel.Interfaces;
using TimMovie.SharedKernel.Validators;

namespace TimMovie.Core.Services.Films;

public class FilmCardService
{
    private readonly IRepository<Film> _filmRepository;
    private readonly IRepository<UserFilmWatched> _userFilmWatchedRepository;
    private readonly IMapper _mapper;
    private readonly FilmService _filmService;


    public FilmCardService(
        IRepository<Film> filmRepository, 
        IMapper mapper,
        FilmService filmService,
        IRepository<UserFilmWatched> userFilmWatchedRepository)
    {
        _filmRepository = filmRepository;
        _mapper = mapper;
        _filmService = filmService;
        _userFilmWatchedRepository = userFilmWatchedRepository;
    }

    public IEnumerable<FilmCardDto> GetFilmCardsByFilters(
        GeneralPaginationDto<SelectedFilmFiltersDto> filtersWithPagination)
    {
        ArgumentValidator.ThrowExceptionIfNull(filtersWithPagination, nameof(filtersWithPagination));
        ArgumentValidator.ThrowExceptionIfNull(
            filtersWithPagination.DataDto,
            nameof(filtersWithPagination.DataDto));

        var filters = filtersWithPagination.DataDto;

        var queryExecutor = GetQueryExecutorWithFiltersAndSort(filters);

        var films = GetFilmsWithCountryAndGenres(filtersWithPagination, queryExecutor);

        var filmCard = GetFilmCardsByFilms(films);

        return filmCard;
    }

    private List<FilmCardDto> GetFilmCardsByFilms(IEnumerable<Film> films)
    {
        return films
            .Select(film =>
            {
                var filmCard = _mapper.Map<FilmCardDto>(film);
                filmCard.Rating = _filmService.GetRating(film);
                filmCard.IsExistInSubscribe = _filmService.IsExistInSubscribe(film);
                return filmCard;
            })
            .ToList();
    }

    private static IEnumerable<Film> GetFilmsWithCountryAndGenres(
        GeneralPaginationDto<SelectedFilmFiltersDto> filtersWithPagination,
        QueryExecutor<Film> queryExecutor)
    {
        return queryExecutor
            .IncludeInResult(film => film.Genres)
            .IncludeInResult(film => film.Country)
            .GetEntitiesWithPagination(filtersWithPagination.AmountSkip, filtersWithPagination.AmountTake);
    }

    private QueryExecutor<Film> GetQueryExecutorWithFiltersAndSort(SelectedFilmFiltersDto filters)
    {
        var filterBuilder = new FilmFiltersBuilder(_filmRepository);
        filterBuilder
            .AddFilterByGenre(filters.GenresName)
            .AddFilterByYear(filters.AnnualPeriod.FirstYear, filters.AnnualPeriod.LastYear)
            .AddFilterByCountry(filters.CountriesName);
        //.FilterOnMinimumRating(filters.Rating ?? 0);//Надо добавить рейтинг в бд

        var sortBuilder = new SortFilmBuilder(filterBuilder);
        sortBuilder.AddSortByTypeSort(filters.SortingType, filters.IsDescending);

        return sortBuilder.Build();
    }


    public List<FilmCardDto> GetFilmCardsByGenre(string genreName, int amount)
    {
        var filterBuilder = new FilmFiltersBuilder(_filmRepository);
        filterBuilder.AddFilterByGenre(new[] {genreName});

        var sortBuilder = new SortFilmBuilder(filterBuilder);
        sortBuilder.AddSortByRating(false);
        sortBuilder.AddSortByViews(false);

        var queryExecutor = sortBuilder.Build();

        var films = queryExecutor
            .IncludeInResult(film => film.Genres)
            .IncludeInResult(film => film.Country)
            .GetEntitiesWithPagination(0, amount);
        return GetFilmCardsByFilms(films);
    }

    public IEnumerable<FilmCardDto> GetLatestFilmsViewedByUser(Guid userId, int amount)
    {
        var query = _userFilmWatchedRepository.Query
            .Where(watched => watched.WatchedUser.Id == userId)
            .OrderByDescending(watched => watched.Date);
        var queryExec = new QueryExecutor<UserFilmWatched>(query, _userFilmWatchedRepository);
        
        var films = queryExec
            .IncludeInResult(watched => watched.Film)
            .GetEntitiesWithPagination(0, amount)
            .Select(watched => watched.Film);
        
        return GetFilmCardsByFilms(films);
    }
}