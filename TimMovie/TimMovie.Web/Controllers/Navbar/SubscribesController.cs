using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Services.Subscribes;
using TimMovie.Web.Extensions;
using TimMovie.Web.ViewModels.SearchFromLayout;

namespace TimMovie.Web.Controllers.Navbar;

public class SubscribesController : Controller
{
    private readonly SubscribeService _subscribeService;

    public SubscribesController(SubscribeService subscribeService)
    {
        _subscribeService = subscribeService;
    }
    
    [HttpGet]
    public IActionResult Subscribes() => View("~/Views/Subscribes/Subscribes.cshtml");
    
    [HttpPost]
    public IActionResult SubscribesResult(string namePart, int take = int.MaxValue, int skip = 0)
    {
        var subscribes = _subscribeService.GetSubscribesByNamePart(namePart, take, skip);
        var userSubscribes = _subscribeService.GetAllActiveUserSubscribes(User.GetUserId());
        var viewModel = new SearchSubscribeViewModel
        {
            Subscribes = subscribes,
            UserSubscribes = userSubscribes
        };
        return View("~/Views/Subscribes/SubscribesResult.cshtml", viewModel);
    }
}