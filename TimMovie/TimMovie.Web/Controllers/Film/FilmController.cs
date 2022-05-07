using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Services.Films;
using TimMovie.Web.ViewModels;

namespace TimMovie.Web.Controllers.Film;

public class FilmController: Controller
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
}