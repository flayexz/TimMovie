using System.Net.Mail;
using TimMovie.Core.Classes;
using TimMovie.Core.Interfaces;
using TimMovie.Infrastructure.Services;
using TimMovie.Infrastructure.StartupSetup;

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
        
        // services.AddAuthentication().AddVKontakte(
        //     opt =>
        //     {
        //         opt.ClientId = configuration.GetValue<string>("VkSettings:ClientId");
        //         opt.ClientSecret = configuration.GetValue<string>("VkSettings:ClientSecret");
        //     });
        
        services.AddAuthorization();
        services.AddControllersWithViews();
        services.AddAutoMapper(typeof(AppMappingProfile));
        
        
        //TODO: сделать чтобы было вот так
       // services.Configure<MailSetup>(configuration.GetSection("MailSetup"));
       services.AddScoped<IMailService,MailKitService>(o => new MailKitService(new MailSetup(
            configuration.GetValue<int>("MailSetup:Port"),
            configuration.GetSection("MailSetup:Host").Value,
            configuration.GetSection("MailSetup:Password").Value,
            configuration.GetSection("MailSetup:FromCompanyName").Value,
            configuration.GetSection("MailSetup:FromCompanyAddress").Value)));
       services.AddScoped<IUserMessageService,UserMessageService>();
       return services;
    }
}