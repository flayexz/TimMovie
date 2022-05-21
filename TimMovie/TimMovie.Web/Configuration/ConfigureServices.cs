using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using TimMovie.Core;
using TimMovie.Core.Classes;
using TimMovie.Infrastructure;
using TimMovie.Web.AuthorizationHandlers.AgePolicy;

namespace TimMovie.Web.Configuration;

public static class ServicesConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.AddDistributedMemoryCache();

        if (environment.IsDevelopment())
        {
            services.AddDbContext(configuration.GetConnectionString("DefaultConnection"));
        }
        else
        {
            var connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL")!;

            var parsedUrl = connectionUrl.Split(";");

            var host = parsedUrl[0].Split("=")[1];
            var username = parsedUrl[1].Split("=")[1];
            var password = parsedUrl[2].Split("=")[1];
            var database = parsedUrl[3].Split("=")[1];

            var defaultConnectionString = $"Host={host};Database={database};Username={username};Password={password};";
            services.AddDbContext(defaultConnectionString);
        }

        services.AddIdentity();
        services.AddSignalR(options => { options.ClientTimeoutInterval = new TimeSpan(0, 5, 0); });

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "auth";
            options.LoginPath = new PathString("/Account/Registration");
            options.AccessDeniedPath = new PathString("/Account/Denied");
        });

        if (environment.IsDevelopment())
        {
            services.AddAuthentication().AddVkontakte(configuration["VkSettings:AppId"],
                configuration["VkSettings:AppSecret"]);
        }
        else
        {
            var vkAppId = Environment.GetEnvironmentVariable("VK_APP_ID")!;
            var vkAppSecret = Environment.GetEnvironmentVariable("VK_APP_SECRET")!;
            services.AddAuthentication().AddVkontakte(vkAppId, vkAppSecret);
        }

        services.AddTransient<IAuthorizationHandler, AgeHandler>();
        services.AddAuthorization(opt =>
            opt.AddPolicy("AtLeast18", policy => policy.Requirements.Add(new AgeRequirement(18))));

        services.AddControllersWithViews();

        services.AddAutoMapper(
            typeof(AppMappingProfile),
            typeof(CoreMappingProfile),
            typeof(InfrastructureMappingProfile));
        services.AddTransient(typeof(Lazy<>), typeof(Lazier<>));

        if (environment.IsDevelopment())
        {
            services.Configure<MailSetup>(configuration.GetSection("MailSetup"));
            services.AddScoped(x => x.GetService<IOptions<MailSetup>>()!.Value);
        }
        else
        {
            var mailPort = int.Parse(Environment.GetEnvironmentVariable("MAIL_PORT")!);
            var mailHost = Environment.GetEnvironmentVariable("MAIL_HOST")!;
            var mailPassword = Environment.GetEnvironmentVariable("MAIL_PASSWORD")!;
            var mailCompanyName = Environment.GetEnvironmentVariable("MAIL_FROM_COMPANY_NAME")!;
            var mailCompanyAddress = Environment.GetEnvironmentVariable("MAIL_FROM_COMPANY_ADDRESS")!;
            services.AddScoped<MailSetup>(x =>
                new MailSetup(mailPort, mailHost, mailPassword, mailCompanyName, mailCompanyAddress));
        }

        return services;
    }
}