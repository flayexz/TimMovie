using System.Net;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using TimMovie.Core;
using TimMovie.Infrastructure;
using TimMovie.Web.Configuration;
using TimMovie.Web.Extensions;
using TimMovie.Web.gRPC;
using TimMovie.Web.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 5011, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
    });
});

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.ConfigureServices(builder.Configuration,builder.Environment);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<CoreModule>();
    containerBuilder.RegisterModule(new InfrastructureModule(builder.Configuration));
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseFileService();
app.UseStaticFiles();

app.UseRouting();

app.UseGrpcWeb();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.UseBannedUserService();

//app.UseUserStatusUpdateService();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=MainPage}/{action=MainPage}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<ChatService>().EnableGrpcWeb().RequireCors("AllowAll");
    endpoints.MapHub<ChatHub>("/chat");
});
app.MapGraphQL();

app.Run();
