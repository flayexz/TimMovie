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
    
    
    
    [HttpGet("[controller]/{userId:guid}")]
    public IActionResult WatchLater(Guid userId)
    {
        var isOwner = User.Identity.IsAuthenticated && User.GetUserId() == userId;
        if (!isOwner)
        {
            return BadRequest();
        }
        var watchLaterFilms =  _watchLaterService.GetWatchLaterFilmsAsync(userId);
        
        var watchedFilmsList = _mapper.Map<List<BigFilmCardViewModel>>(watchLaterFilms);
        return View("~/Views/Navbar/WatchLater/WatchLater.cshtml", watchedFilmsList);
    }
}