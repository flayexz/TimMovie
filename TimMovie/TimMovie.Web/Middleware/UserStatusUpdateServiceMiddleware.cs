using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimMovie.Core.Entities;
using TimMovie.Core.Entities.Enums;
using TimMovie.Core.Services.Films;
using TimMovie.Core.Specifications.InheritedSpecifications;
using TimMovie.Infrastructure.Database;
using TimMovie.Web.Extensions;

namespace TimMovie.Web.Middleware;

public class UserStatusUpdateServiceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly UserManager<User> _userManager;
    private readonly FilmService _filmService;

    public UserStatusUpdateServiceMiddleware(RequestDelegate next,
        UserManager<User> userManager, FilmService filmService)
    {
        _next = next;
        _userManager = userManager;
        _filmService = filmService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value;
        var userId = context.User.GetUserId();
        if (userId is not null)
        {
            if (path is not null && !path.Contains("UpdateUserStatusWatchingFilm"))
            {
                if (path.Contains("Film/") && Guid.TryParse(path.Split("/")[^1], out var filmId))
                {
                    var film = _filmService.GetDbFilmById(filmId);
                    if (film is not null)
                        await UpdateUserStatus(userId.Value, UserStatusEnum.Watching, film);
                }
                else
                    await UpdateUserStatus(userId.Value, UserStatusEnum.Online);
            }

            if (path is null)
                await UpdateUserStatus(userId.Value, UserStatusEnum.Online);
        }

        await _next.Invoke(context);
    }

    private async Task UpdateUserStatus(Guid userId, UserStatusEnum userStatusEnum, Film? film = null)
    {
        var dbUser = await _userManager.Users.Include(u => u.Status).FirstOrDefaultAsync(new EntityByIdSpec<User>(userId));
        if (dbUser is not null)
        {
            dbUser.Status ??= new UserStatus();
            if (film is not null)
                dbUser.WatchingFilm = film;
            dbUser.Status.UserStatusEnum = userStatusEnum;
            dbUser.Status.DateLastChange = DateTime.Now;
            await _userManager.UpdateAsync(dbUser);
        }
    }
}