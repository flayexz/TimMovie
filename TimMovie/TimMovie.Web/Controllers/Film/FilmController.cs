using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using TimMovie.Core.DTO.Comments;
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
    private Guid? UserId => User.GetUserId();


    public FilmController(IMapper mapper, FilmService filmService, UserManager<User> userManager)
    {
        _mapper = mapper;
        _filmService = filmService;
        _userManager = userManager;
    }


    [HttpGet("[controller]/{filmId:guid}")]
    public async Task<IActionResult> Film(Guid filmId)
    {
        var film = _mapper.Map<FilmViewModel>(_filmService.GetFilmById(filmId));
        if (UserId is not null)
        {
            film.IsGradeSet = GetGrade(filmId) is not null;
            film.IsAddedToWatchLater = _filmService.IsWatchLaterFilm(filmId, UserId.Value);
        }

        if (UserId is null) return View("~/Views/Film/Film.cshtml", film);
        var user = await _userManager.FindByIdAsync(UserId.ToString());
        film.PathToUserPhoto = user.PathToPhoto;
        var comments = GetCommentsWithPagination(film.Id, 0, MaxTakeValue)?.ToList();
        if (comments is null) return View("~/Views/Film/Film.cshtml", film);
        var commentsDto = _mapper.Map<List<CommentsDto>>(comments);
        film.Comments = commentsDto;
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
        var commentsDto = _mapper.Map<List<CommentsDto>>(comments);
        return View("~/Views/Partials/Film/CommentsPartial.cshtml", commentsDto);
    }


    [HttpPost]
    public async Task<IActionResult> LeaveComment(Guid filmId, string content)
    {
        var result = await _filmService.TryAddCommentToFilm(User.GetUserId(), filmId, content);
        if (result.IsFailure)
            return BadRequest();
        var commentDto = _mapper.Map<CommentsDto>(result.Value);
        return View("~/Views/Partials/Film/CommentsPartial.cshtml", new List<CommentsDto> {commentDto});
    }

    [HttpPost]
    public int? GetGrade(Guid filmId) =>
        !_filmService.TryGetUserGrade(filmId, UserId.Value, out var grade)
            ? null
            : grade;


    [HttpPost]
    public async Task<IActionResult> SetGrade(Guid filmId, int grade)
    {
        if (grade is < 1 or > 10)
            return BadRequest();
        if (UserId is null)
            return BadRequest();
        if (!await _filmService.TryUpdateFilmGrade(filmId, UserId.Value, grade))
            return BadRequest();
        return Ok();
    }

    private IEnumerable<Comment>? GetCommentsWithPagination(Guid filmId, int skip, int take)
    {
        var film = _filmService.GetDbFilmById(filmId);
        var result = film?.Comments.OrderByDescending(c => c.Date).Skip(skip).Take(take).ToList();
        return result;
    }

    [HttpPost]
    public async Task<IActionResult> AddFilmToWatchLater(Guid filmId)
    {
        if (UserId is null)
            return BadRequest();
        await _filmService.TryAddFilmToWatchLater(filmId, UserId.Value);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFilmFromWatchLater(Guid filmId)
    {
        if (UserId is null)
            return BadRequest();
        await _filmService.TryRemoveFilmFromWatchLater(filmId, UserId.Value);
        return Ok();
    }
}