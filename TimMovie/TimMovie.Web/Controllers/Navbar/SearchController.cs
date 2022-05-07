using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Services.Subscribes;
using TimMovie.Web.ViewModels.SearchFromLayout;

namespace TimMovie.Web.Controllers.Navbar;

public class SearchController : Controller
{
    private readonly SubscribeService _subscribeService;

    public SearchController(SubscribeService subscribeService)
    {
        _subscribeService = subscribeService;
    }
    
    [HttpGet]
    public IActionResult Search() => View("~/Views/Subscribes/Subscribes.cshtml");

    [HttpPost]
    public IActionResult SearchEntityResults(string namePart) =>
        View("/Views/Navbar/SearchEntity/SearchEntityResult.cshtml", namePart);
    
    [HttpPost]
    public IActionResult SearchSubscribeResults(string namePart, int take = int.MaxValue, int skip = 0)
    {
        var subscribes = _subscribeService.GetSubscribesByNamePart(namePart, take, skip);
        var viewModel = new SearchSubscribeViewModel
        {
            Subscribes = subscribes
        };
        return View("~/Views/Subscribes/SubscribesResult.cshtml", viewModel);
    }

}