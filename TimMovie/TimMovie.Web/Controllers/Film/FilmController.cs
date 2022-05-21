using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Services.Films;
using TimMovie.Web.Extensions;
using TimMovie.Web.ViewModels;

namespace TimMovie.Web.Controllers.Film;

[ApiExplorerSettings(IgnoreApi=true)]
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
    public string? GetGrade(Guid filmId)
    {
        var userId = User.GetUserId();
        if (userId is null)
            return $"{nameof(AccountController)}/{nameof(AccountController.Registration)}".Replace("Controller", "");
        return !_filmService.TryGetUserGrade(filmId, userId.Value, out var grade) ? null : grade.ToString();
    }

    [HttpPost]
    public async Task<IActionResult> SetGrade(Guid filmId, int grade)
    {
        if (grade is < 1 or > 10)
            return NotFound();
        if (!await _filmService.TryUpdateFilmGrade(filmId, User.GetUserId().Value, grade))
            return NotFound();
        return Ok();
    }
}