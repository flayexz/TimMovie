using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Services.Films;
using TimMovie.Web.Extensions;
using TimMovie.Web.ViewModels;

namespace TimMovie.Web.Controllers.Film;

[ApiExplorerSettings(IgnoreApi = true)]
public class FilmController : Controller
{
    private readonly IMapper _mapper;
    private readonly FilmService _filmService;


    public FilmController(IMapper mapper, FilmService filmService)
    {
        _mapper = mapper;
        _filmService = filmService;
    }

    [HttpGet("[controller]/{id:guid}")]
    public IActionResult Film(Guid id)
    {
        var film = _mapper.Map<FilmViewModel>(_filmService.GetFilmById(id));

        return View("~/Views/Film/Film.cshtml", film);
    }

    [HttpPost]
    public int? GetGrade(Guid filmId) =>
        !_filmService.TryGetUserGrade(filmId, User.GetUserId().Value, out var grade) ? null : grade;

    [HttpPost]
    public async Task<IActionResult> SetGrade(Guid filmId, int grade)
    {
        if (grade is < 1 or > 10)
            return NotFound();
        return !await _filmService.TryUpdateFilmGrade(filmId, User.GetUserId().Value, grade)
            ? NotFound()
            : Ok();
    }

    [HttpPost]
    public async Task<IActionResult> AddToWatchLater(Guid filmId) =>
        !await _filmService.TryAddFilmToWatchLater(filmId, User.GetUserId().Value)
            ? NotFound()
            : Ok();

    public async Task<IActionResult> RemoveFromWatchLater(Guid filmId) =>
        !await _filmService.TryRemoveFilmFromWatchLater(filmId, User.GetUserId().Value)
            ? NotFound()
            : Ok();
}