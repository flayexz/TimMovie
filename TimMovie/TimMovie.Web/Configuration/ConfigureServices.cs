using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using TimMovie.Core.Classes;
using TimMovie.Core.Interfaces;
using TimMovie.Infrastructure.Database;
using TimMovie.Infrastructure.Database.Repositories;
using TimMovie.Infrastructure.Identity;
using TimMovie.Infrastructure.Services;
using TimMovie.Infrastructure.StartupSetup;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Web.Configuration;

public static class ServicesConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext(configuration.GetConnectionString("DefaultConnection"));
        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/Denied";
        });
        services.AddAuthentication().AddVkontakte(configuration);
        
        services.AddTransient<IAuthorizationHandler, AgeHandler>();
        services.AddAuthorization(opt =>
            opt.AddPolicy("AtLeast18", policy => policy.Requirements.Add(new AgeRequirement(18))));
        
        services.AddControllersWithViews();
        services.AddAutoMapper(typeof(AppMappingProfile));

        services.AddScoped<CountryRepository>();
        
        services.Configure<MailSetup>(configuration.GetSection("MailSetup"));
       services.AddScoped(x => x.GetService<IOptions<MailSetup>>()!.Value);
       services.AddScoped<IMailService,MailKitService>();
       
       services.AddScoped<IUserMessageService, UserMessageService>();
       services.AddTransient<IIpService,IpService>();

       services.AddTransient<IVkService, VkService>(opt =>
           new VkService(configuration["VkSettings:AccessToken"], new HttpClient()));
       return services;
    }
}