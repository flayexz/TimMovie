using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using TimMovie.Core.Services.Actors;
using TimMovie.Core.Services.Films;
using TimMovie.Core.Services.Genres;
using TimMovie.Core.Services.Producers;
using TimMovie.Web.ViewModels.SearchFromLayout;

namespace TimMovie.Web.ViewComponents;

public class SearchEntityViewComponent : ViewComponent
{
    private readonly FilmService _filmService;
    private readonly ActorService _actorService;
    private readonly ProducerService _producerService;
    private readonly GenreService _genreService;

    public SearchEntityViewComponent(FilmService filmService, ActorService actorService, ProducerService producerService,
        GenreService genreService)
    {
        _filmService = filmService;
        _actorService = actorService;
        _producerService = producerService;
        _genreService = genreService;
    }

    /// <summary>
    /// Всего 10 записей: 4 фильма, 2 жанра, 2 актера, 2 режиссера
    /// </summary>
    public async Task<IViewComponentResult> InvokeAsync(string namePart)
    {
        var films = _filmService.GetFilmsByNamePart(namePart, 4);
        var genres = _genreService.GetGenresByNamePart(namePart, 2);
        var actors = _actorService.GetActorsByNamePart(namePart, 2);
        var producers = _producerService.GetProducersByNamePart(namePart, 2);

        var viewModel = new SearchEntityViewModel
        {
            Films = films.Select(f => f.Title),
            Genres = genres.Select(g => g.Name),
            Actors = actors.Select(a => a.Surname is null ? $"{a.Name}" : $"{a.Name} {a.Surname}"),
            Producers = producers.Select(p => p.Surname is null ? $"{p.Name}" : $"{p.Name} {p.Surname}")
        };
        return View("~/Views/Shared/Components/SearchEntity/Default.cshtml", viewModel);
    }
}