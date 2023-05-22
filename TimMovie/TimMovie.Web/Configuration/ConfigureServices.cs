using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using TimMovie.Core;
using TimMovie.Core.Classes;
using TimMovie.Infrastructure;
using TimMovie.Web.AuthorizationHandlers.AgePolicy;
using TimMovie.Web.GraphQL.Query;

namespace TimMovie.Web.Configuration;

public static class ServicesConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddDistributedMemoryCache();
        services.AddDbContext(environment.IsDevelopment()
            ? configuration.GetConnectionString("DefaultConnection")
            : Environment.GetEnvironmentVariable("DATABASE_URL")!);
        services.AddIdentity();
        services.AddSignalR(options => { options.ClientTimeoutInterval = new TimeSpan(0, 5, 0); });

        services.Configure<MailSetup>(configuration.GetRequiredSection("MailSetup"));
        services.AddScoped(x => x.GetService<IOptions<MailSetup>>()!.Value);
        
        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "auth";
            options.LoginPath = new PathString("/Account/Registration");
            options.AccessDeniedPath = new PathString("/Account/Denied");
        });

        services.AddAuthentication().AddVkontakte(configuration.GetRequiredSection("VkSettings:AppId").Value!,
                configuration.GetRequiredSection("VkSettings:AppSecret").Value!);
        
        services.AddTransient<IAuthorizationHandler, AgeHandler>();
        services.AddAuthorization(opt =>
            opt.AddPolicy("AtLeast18", policy => policy.Requirements.Add(new AgeRequirement(18))));

        services.AddControllersWithViews();

        AddGraphQL(services);

        services.AddAutoMapper(
            typeof(AppMappingProfile),
            typeof(CoreMappingProfile),
            typeof(InfrastructureMappingProfile));
        services.AddTransient(typeof(Lazy<>), typeof(Lazier<>));

        return services;
    }

    private static void AddGraphQL(IServiceCollection services)
    {
        services
            .AddGraphQLServer()
            .AddQueryType<RootQuery>();
    }
}