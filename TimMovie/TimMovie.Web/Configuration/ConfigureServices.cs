using Microsoft.AspNetCore.Identity;
using TimMovie.Core.Entities;
using TimMovie.Infrastructure.StartupSetup;

namespace TimMovie.Web.Configuration;

public static class ServicesConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext(configuration.GetConnectionString("DefaultConnection"));
        services.AddControllersWithViews();
        services.AddAutoMapper(typeof(AppMappingProfile));
        return services;
    }
}