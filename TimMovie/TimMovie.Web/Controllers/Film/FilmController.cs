using Microsoft.AspNetCore.Mvc;

namespace TimMovie.Web.Controllers.Film;

public class FilmController: Controller
{
    [HttpGet("[controller]/{id:guid}")]
    public IActionResult Film(Guid id)
    {
        return View("~/Views/Film/Film.cshtml");
    }
}