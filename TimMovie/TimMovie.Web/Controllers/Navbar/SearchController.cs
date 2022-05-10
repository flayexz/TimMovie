using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Services.Subscribes;

namespace TimMovie.Web.Controllers.Navbar;

public class SearchController : Controller
{
    [HttpPost]
    public IActionResult SearchEntityResults(string namePart) =>
        View("/Views/Navbar/SearchEntity/SearchEntityResult.cshtml", namePart);
}