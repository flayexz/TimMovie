using Autofac;
using Autofac.Extensions.DependencyInjection;
using TimMovie.Core;
using TimMovie.Infrastructure;
using TimMovie.Infrastructure.Services;
using TimMovie.Web.Configuration;
using TimMovie.Web.Extensions;
using TimMovie.Web.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.ConfigureServices(builder.Configuration);

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
app.UseUserStatusService();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=MainPage}/{action=MainPage}/{id?}");
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chat");
});

app.Run();