using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimMovie.Core.Entities;
using TimMovie.Core.Entities.Enums;
using TimMovie.Infrastructure.Database;
using TimMovie.Web.Extensions;

namespace TimMovie.Web.Middleware;

public class UserStatusDeleteServiceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly UserManager<User> _userManager;
    private const int CountMinutes = 5;

    public UserStatusDeleteServiceMiddleware(RequestDelegate next, UserManager<User> userManager)
    {
        _next = next;
        _userManager = userManager;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var users = _userManager.Users
            .Include(u => u.Status)
            .Where(u => u.Status != null && u.Status.UserStatusEnum != UserStatusEnum.Offline);
        foreach (var user in users)
        {
            if (DateTime.Now - user.Status!.DateLastChange <= TimeSpan.FromMinutes(CountMinutes)) continue;
            user.Status.UserStatusEnum = UserStatusEnum.Offline;
            user.Status.DateLastChange = DateTime.Now;
            await _userManager.UpdateAsync(user);
        }

        await _next.Invoke(context);
    }
}