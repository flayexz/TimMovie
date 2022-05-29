using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Services.Films;
using TimMovie.Web.Extensions;
using TimMovie.Web.ViewModels.FilmCard;

namespace TimMovie.Web.Controllers.WatchLaterController;

public class WatchLaterController : Controller
{
    private readonly IMapper _mapper;
    private readonly WatchLaterService _watchLaterService;

    public WatchLaterController(IMapper mapper, WatchLaterService watchLaterService)
    {
        _mapper = mapper;
        _watchLaterService = watchLaterService;
    }

    [HttpGet("[controller]")]
    public IActionResult WatchLater()
    {
        return View("~/Views/Navbar/WatchLater/WatchLater.cshtml");
    }


    [HttpPost]
    public async Task<IActionResult> WatchLaterFilms(int take, int skip)
    {
        var userId = User.GetUserId();
        if (userId is null)
            return BadRequest();
        var isOwner = User.Identity.IsAuthenticated;
        if (!isOwner) return BadRequest();
        
        var watchLaterFilms = _watchLaterService.GetWatchLaterFilmsAsync(userId.Value, take, skip);

        var cardsViewModel = _mapper.Map<IEnumerable<BigFilmCardViewModel>>(watchLaterFilms);

        return PartialView("~/Views/Partials/FilmCard/BigFilmCards.cshtml", cardsViewModel);
    }
}