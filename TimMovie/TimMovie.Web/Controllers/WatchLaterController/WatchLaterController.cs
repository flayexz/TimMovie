using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Services.Films;
using TimMovie.Web.Extensions;
using TimMovie.Web.ViewModels;
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
    
    
    
    [HttpGet("[controller]/{id:guid}")]
    public async Task<IActionResult> WatchLater(Guid userId)
    {
        var isOwner = User.Identity.IsAuthenticated && User.GetUserId() == userId;
        if (!isOwner)
        {
            return BadRequest();
        }
        var watchLaterFilms =  await _watchLaterService.GetWatchLaterFilmsAsync(userId);
        if (!watchLaterFilms.Any())
            return NotFound();
        
        var watchedFilmsList = _mapper.Map<List<BigFilmCardViewModel>>(watchLaterFilms);
        return View("~/Views/Navbar/WatchLater/WatchLater.cshtml", watchedFilmsList);
    }
}