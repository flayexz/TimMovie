using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TimMovie.Infrastructure.Database;

namespace TimMovie.Infrastructure.StartupSetup;

public static class StartupSetup
{
    public static void AddDbContext(this IServiceCollection services, string connectionString) =>
        services.AddDbContext<ApplicationContext>(options =>
            options.UseNpgsql(connectionString));
}