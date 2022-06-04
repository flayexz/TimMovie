using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using TimMovie.Core.Const;
using TimMovie.Core.Entities;
using TimMovie.Core.Entities.Enums;
using TimMovie.Web.Extensions;

namespace TimMovie.Web.Middleware;

public class BannedUserMiddleware
{
    private readonly RequestDelegate _next;

    public BannedUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        var isBanned = context.User.HasRoleClaim(RoleNames.Banned);
        if (isBanned && context.Request.Path.Value != "/Errors/PageForBannedUser")
        {
            await signInManager.SignOutAsync();
            context.Response.Redirect("/Errors/PageForBannedUser");
            return;
        }
        await _next.Invoke(context);
    }
}