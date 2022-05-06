using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.DTO;
using TimMovie.Core.DTO.Films;
using TimMovie.Core.Services;
using TimMovie.Core.Services.Countries;
using TimMovie.Core.Services.Films;
using TimMovie.Core.Services.Genres;
using TimMovie.Web.ViewModels.FilmCard;
using TimMovie.Web.ViewModels.FilmFilter;

namespace TimMovie.Web.Controllers.Navbar;

public class FilmsController : Controller
{
    private readonly CountryService _countryService;
    private readonly GenreService _genreService;
    private readonly FilmCardService _filmCardService;
    private readonly IMapper _mapper;

    public FilmsController(
        FilmCardService filmsFilterService,
        CountryService countryService,
        GenreService genreService,
        IMapper mapper)
    {
        _filmCardService = filmsFilterService;
        _countryService = countryService;
        _genreService = genreService;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult Filters()
    {
        var filmFilters = new FilmFiltersViewModel
        {
            GenresName = _genreService.GetGenreNames(),
            CountriesName = _countryService.GetCountryNames(),
            Ratings = new[] {9, 8, 7, 6, 5},
            AnnualPeriods = new[]
            {
                new AnnualPeriodDto(2022, 2022),
                new AnnualPeriodDto(2021, 2021),
                new AnnualPeriodDto(2020, 2020),
                new AnnualPeriodDto(2019, 2017),
                new AnnualPeriodDto(2016, 2014),
                new AnnualPeriodDto(2013, 2011),
                new AnnualPeriodDto(2010, 2001),
                new AnnualPeriodDto(2000, 1900)
            }
        };

        return View("/Views/Navbar/Films/Filters.cshtml", filmFilters);
    }

    [HttpPost]
    public IActionResult FilmFilters(GeneralPaginationDto<SelectedFilmFiltersDto> filtersWithPagination)
    {
        var cardsDto = _filmCardService.GetFilmCardsByFilters(filtersWithPagination);

        var cardsViewModel = _mapper.Map<IEnumerable<FilmCardViewModel>>(cardsDto);

        return PartialView("~/Views/Partials/FilmCard/FilmCards.cshtml", cardsViewModel);
    }
}