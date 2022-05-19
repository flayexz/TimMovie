using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimMovie.Core.Entities;
using TimMovie.Core.Entities.Enums;
using TimMovie.Web.Extensions;

namespace TimMovie.Web.Middleware;

public class UserStatusServiceMiddleware
{
    private readonly ILogger<UserStatusServiceMiddleware> _logger;
    private readonly RequestDelegate _next;
    private readonly UserManager<User> _userManager;

    public UserStatusServiceMiddleware(ILogger<UserStatusServiceMiddleware> logger, RequestDelegate next,
        UserManager<User> userManager)
    {
        _logger = logger;
        _next = next;
        _userManager = userManager;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value;
        var userId = context.User.GetUserId();
        if (userId is not null && path is not null)
        {
            if (path.Contains("Film"))
                await UpdateUserStatus(userId.Value, UserStatusEnum.Watching);
            else
                await UpdateUserStatus(userId.Value, UserStatusEnum.Online);
        }

        await _next.Invoke(context);
    }

    private async Task UpdateUserStatus(Guid userId, UserStatusEnum userStatusEnum)
    {
        var dbUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (dbUser is not null)
        {
            dbUser.Status ??= new UserStatus();
            dbUser.Status.UserStatusEnum = userStatusEnum;
            dbUser.Status.DateLastChange = DateTime.Now;
        }

        await _userManager.UpdateAsync(dbUser);
        _logger.LogInformation($"Статус пользователя обновлен мидлтварью");
    }
}