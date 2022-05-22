using Autofac;
using Autofac.Extensions.DependencyInjection;
using TimMovie.Core;
using TimMovie.Infrastructure;
using TimMovie.Infrastructure.Services;
using TimMovie.Web.Configuration;
using TimMovie.Web.Extensions;
using TimMovie.Web.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

var defaultConnectionString = string.Empty;

if (builder.Environment.EnvironmentName == "Development") {
    defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}
else
{
    // Use connection string provided at runtime by Heroku.
    var connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

    var parsedUrl = connectionUrl.Split(";");
    var host = parsedUrl[0].Split("=")[1];
    var user = parsedUrl[1].Split("=")[1];
    var password = parsedUrl[2].Split("=")[1];
    var database = parsedUrl[3].Split("=")[1];

    defaultConnectionString = $"Host={host};Database={database};Username={user};Password={password};SSL Mode=Require;Trust Server Certificate=true";
}

builder.Services.AddDbContext(defaultConnectionString);

builder.Services.ConfigureServices(builder.Configuration);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<CoreModule>();
    containerBuilder.RegisterModule(new InfrastructureModule(builder.Configuration));
});

builder.Services.AddHostedService<UserStatusWorker>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


//app.UseHttpsRedirection();

app.UseFileService();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseUserStatusUpdateService();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=MainPage}/{action=MainPage}/{id?}");
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chat");
});

app.Run();