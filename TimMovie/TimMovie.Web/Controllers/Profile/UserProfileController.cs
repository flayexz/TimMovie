using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Interfaces;
using TimMovie.Core.Services.Films;
using TimMovie.Core.Services.Subscribes;
using TimMovie.Web.Extension;
using TimMovie.Web.ViewModels.FilmCard;
using TimMovie.Web.ViewModels.User;
using TimMovie.Web.ViewModels.UserSubscribes;

namespace TimMovie.Web.Controllers.Profile;

public class UserProfileController : Controller
{
    private readonly IUserService _userService;
    private readonly FilmCardService _filmCardService;
    private readonly SubscribeService _subscribeService;
    private readonly IMapper _mapper;

    public UserProfileController(
        IUserService userService,
        IMapper mapper,
        FilmCardService filmCardService,
        SubscribeService subscribeService)
    {
        _userService = userService;
        _mapper = mapper;
        _filmCardService = filmCardService;
        _subscribeService = subscribeService;
    }

    [HttpGet("[controller]/{id:guid}")]
    public async Task<IActionResult> Profile(Guid id)
    {
        if (!await _userService.UserIsExisted(id))
        {
            return View("~/Views/Errors/UserNotExisting.cshtml");
        }

        var userProfile = new UserProfileViewModel
        {
            IsOwner = User.Identity.IsAuthenticated && User.GetUserId() == id,
            UserInfo = _mapper.Map<ShortInfoUserViewModel>(await _userService.GetShortInfoAboutUser(id)),
            FilmCards = _filmCardService
                .GetLatestFilmsViewedByUser(id, 6)
                .Select(cardDto => _mapper.Map<FilmCardViewModel>(cardDto)),
            UserSubscribes = _subscribeService.GetAllUserSubscribes(id)
                .Select(subscribeDto => _mapper.Map<UserSubscribeViewModel>(subscribeDto))
        };
        
        return View("~/Views/Profile/UserProfile.cshtml", userProfile);
    }
}