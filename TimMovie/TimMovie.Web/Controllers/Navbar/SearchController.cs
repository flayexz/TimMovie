using Microsoft.AspNetCore.Mvc;

namespace TimMovie.Web.Controllers.Navbar;

public class SearchController : Controller
{
    [HttpPost]
    public IActionResult SearchResults(string namePart) =>
        View("/Views/Navbar/Search/SearchResult.cshtml", namePart);
}