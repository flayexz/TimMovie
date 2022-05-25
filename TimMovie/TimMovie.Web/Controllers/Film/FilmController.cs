using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Entities;
using TimMovie.Core.Services.Films;
using TimMovie.Web.Extensions;
using TimMovie.Web.ViewModels;

namespace TimMovie.Web.Controllers.Film;

[ApiExplorerSettings(IgnoreApi = true)]
public class FilmController : Controller
{
    private const int MaxTakeValue = 5;
    private readonly IMapper _mapper;
    private readonly FilmService _filmService;
    private readonly UserManager<User> _userManager;



    public FilmController(IMapper mapper, FilmService filmService, UserManager<User> userManager)
    {
        _mapper = mapper;
        _filmService = filmService;
        _userManager = userManager;
    }

    [HttpGet("[controller]/{id:guid}")]
    public async Task<IActionResult> Film(Guid id)
    {
        var film = _mapper.Map<FilmViewModel>(_filmService.GetFilmById(id));
        film.IsGradeSet = GetGrade(id) is not null;
        var userId = User.GetUserId();
        if (userId is null) return View("~/Views/Film/Film.cshtml", film);
        var user = await _userManager.FindByIdAsync(userId.ToString());
        film.PathToUserPhoto = user.PathToPhoto;
        var comments = GetCommentsWithPagination(film.Id, 0, MaxTakeValue)?.ToList();
        if (comments is not null)
            film.Comments = comments
                .Select(comment => { comment.Date -= TimeSpan.FromHours(3); return comment; })
                .ToList();
        return View("~/Views/Film/Film.cshtml", film);
    }

    [HttpPost]
    public IActionResult GetCommentsWithPaginationView(Guid filmId, int take, int skip)
    {
        if (take > MaxTakeValue)
            return BadRequest();
        var comments = GetCommentsWithPagination(filmId, skip, take)?.ToList();
        if (comments is null)
            return BadRequest();
        return View("~/Views/Partials/Film/CommentsPartial.cshtml", comments);
    }


    [HttpPost]
    public async Task<IActionResult> LeaveComment(Guid filmId, string content)
    {
        if (content.Length is < 2 or > 1000)
            return BadRequest();
        var userId = User.GetUserId();
        if (userId is null)
            return BadRequest();
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return BadRequest();
        var dbFilm = _filmService.GetDbFilmById(filmId);
        if (dbFilm is null)
            return BadRequest();
        var comment = new Comment
        {
            Author = user,
            Film = dbFilm,
            Content = content,
            Date = DateTime.Now
        };
        if (!await _filmService.TryAddCommentToFilm(comment))
            return BadRequest();
        return View("~/Views/Partials/Film/CommentsPartial.cshtml", new List<Comment> {comment});
    }

    [HttpPost]
    public int? GetGrade(Guid filmId)
    {
        var userId = User.GetUserId();
        if (userId is null)
            return null;
        return !_filmService.TryGetUserGrade(filmId, userId.Value, out var grade) ? null : grade;
    }

    [HttpPost]
    public async Task<IActionResult> SetGrade(Guid filmId, int grade)
    {
        if (grade is < 1 or > 10)
            return BadRequest();
        var userId = User.GetUserId();
        if (userId is null)
            return BadRequest();
        if (!await _filmService.TryUpdateFilmGrade(filmId, userId.Value, grade))
            return BadRequest();
        return Ok();
    }

    private IEnumerable<Comment>? GetCommentsWithPagination(Guid filmId, int skip, int take)
    {
        var film = _filmService.GetDbFilmById(filmId);
        var result = film?.Comments.OrderByDescending(c => c.Date).Skip(skip).Take(take).ToList();
        return result;
    }
}