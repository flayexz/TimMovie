using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimMovie.Core.Entities;
using TimMovie.Infrastructure.Database;

namespace TimMovie.Web.Configuration;

public static class ConfigureIdentity
{
    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole<Guid>>(opt =>
            {
                opt.Password.RequireUppercase = false;
                opt.Password.RequireDigit = true;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 8;
                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = true;
            })
            .AddSignInManager()
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddDefaultTokenProviders();
        return services;
    } 
}