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
    private readonly IMapper _mapper;
    private readonly FilmService _filmService;

    public FilmCardService(IRepository<Film> filmRepository, IMapper mapper, FilmService filmService)
    {
        _filmRepository = filmRepository;
        _mapper = mapper;
        _filmService = filmService;
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

    private IEnumerable<FilmCardDto> GetFilmCardsByFilms(IEnumerable<Film> films)
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
            .FilterByCountry(filters.CountriesName);
        //.FilterOnMinimumRating(filters.Rating ?? 0);//Надо добавить рейтинг в бд

        var sortBuilder = new SortFilmBuilder(filterBuilder);
        sortBuilder.AddSortByTypeSort(filters.SortingType, filters.IsDescending);

        return sortBuilder.Build();
    }
}