using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Services;
using TimMovie.Web.ViewModels.FilmFilter;
using TimMovie.Web.ViewModels.Films;

namespace TimMovie.Web.Controllers.Navbar;

public class FilmsController : Controller
{
    private readonly FilmService _filmService;
    private readonly CountryService _countryService;
    private readonly GenreService _genreService;
    private readonly FilmsFilterService _filmsFilterService;

    public FilmsController(FilmService filmService, FilmsFilterService filmsFilterService,
        CountryService countryService, GenreService genreService)
    {
        _filmService = filmService;
        _filmsFilterService = filmsFilterService;
        _countryService = countryService;
        _genreService = genreService;
    }

    public IActionResult Page()
    {
        var filmFilters = new FilmFiltersViewModel
        {
            GenresName = _genreService.GetGenreNames(),
            CountriesName = _countryService.GetCountryNames(),
            Ratings = new[] {9, 8, 7, 6, 5},
            AnnualPeriods = new[]
            {
                new AnnualPeriodViewModel(2022, 2022),
                new AnnualPeriodViewModel(2021, 2021),
                new AnnualPeriodViewModel(2020, 2020),
                new AnnualPeriodViewModel(2019, 2017),
                new AnnualPeriodViewModel(2016, 2014),
                new AnnualPeriodViewModel(2013, 2011),
                new AnnualPeriodViewModel(2010, 2001),
                new AnnualPeriodViewModel(2000, 1900)
            }
        };

        return View("/Views/Navbar/Films/Page.cshtml", filmFilters);
    }

    [HttpPost]
    public IActionResult FilmFilters(SelectedFiltersViewModel filters, int pagination, int numberOfLoadedCards)
    {
        var filterBuilder = _filmsFilterService.CreateFilterBuilder();
        filterBuilder
            .FilterByGenre(filters.GenresName)
            .FilterByYear(filters.AnnualPeriodsViewModel.FirstYear, filters.AnnualPeriodsViewModel.LastYear)
            .FilterByCountry(filters.CountriesName);
        //.FilterOnMinimumRating(filters.Rating ?? 0); Надо бд заполнить с рейтингом
        var sortBuilder = _filmsFilterService.CreateSortBuilder(filterBuilder);
        sortBuilder.SortByType(filters.SortingType);

        var films = sortBuilder.Execute(pagination, numberOfLoadedCards);

        var filmCard = films.Select(film => new FilmCardViewModel
        {
            Film = film,
            IsExistInSubscribe = _filmService.IsExistInSubscribe(film),
            Rating = _filmService.GetRating(film) ?? 0
        });

        return PartialView("~/Views/FilmCard/FilmCard.cshtml", filmCard);
    }
}