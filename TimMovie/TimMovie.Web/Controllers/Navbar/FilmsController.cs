using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Services;
using TimMovie.Web.ViewModels;

namespace TimMovie.Web.Controllers.Navbar;

public class FilmsController : Controller
{
    private readonly FilmService _filmService;
    private readonly FilmsFilterService _filmsFilterService;

    public FilmsController(FilmService filmService, FilmsFilterService filmsFilterService)
    {
        _filmService = filmService;
        _filmsFilterService = filmsFilterService;
    }

    public IActionResult Page()
    {
        var filmFilters = new FilmFilters
        {
            GenresName = _filmService.GetGenreNames(),
            CountriesName = _filmService.GetCountryName(),
            Ratings = new[] {9, 8, 7, 6, 5},
            AnnualPeriods = new[]
            {
                new AnnualPeriod(2022, 2022),
                new AnnualPeriod(2021, 2021),
                new AnnualPeriod(2020, 2020),
                new AnnualPeriod(2019, 2017),
                new AnnualPeriod(2016, 2014),
                new AnnualPeriod(2013, 2011),
                new AnnualPeriod(2010, 2001),
                new AnnualPeriod(2000, 1900)
            }
        };

        return View("/Views/Navbar/Films/Page.cshtml", filmFilters);
    }

    [HttpPost]
    public IActionResult FilmFilters(ResultFilters filters, int pagination, int numberOfLoadedCards)
    {
        var filterBuilder = _filmsFilterService.CreateFilterBuilder();
        filterBuilder
            .FilterByGenre(filters.GenresName)
            .FilterByYear(filters.AnnualPeriods.FirstYear, filters.AnnualPeriods.LastYear)
            .FilterByCountry(filters.CountriesName);
            //.FilterOnMinimumRating(filters.Rating ?? 0); Надо бд заполнить с рейтингом
        var sortBuilder = _filmsFilterService.CreateSortBuilder(filterBuilder);
        sortBuilder.SortByType(filters.TypeFilter);
        
        var films = sortBuilder.Execute(pagination, numberOfLoadedCards);

        var filmCard = films.Select(film => new FilmCard
        {
            Film = film,
            IsExistInSubscribe = _filmService.IsExistInSubscribe(film),
            Rating = _filmService.GetRating(film) ?? 0
        });
        
        return PartialView("~/Views/FilmCard/FilmCard.cshtml", filmCard);
    }
}