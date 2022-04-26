namespace TimMovie.WebApi

#nowarn "20"

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.IdentityModel.Logging
open TimMovie.Auth.AuthStartupSetup
open TimMovie.Infrastructure
//open TimMovie.Auth.TestMiddleware

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder(args)
        let services = builder.Services
        let configuration = builder.Configuration

        services.AddControllers()

        services
            .AddCors(fun options ->
                options.AddDefaultPolicy
                    (fun builder ->
                        builder
                            .AllowAnyHeader()
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                        |> ignore))
            .AddAuthenticationAndJwt()
            .AddAuthorization()
            .AddDbContext(configuration["ConnectionStrings:DefaultConnection"])

        services
            .AddIdentity()
            .ConfigureOpenIddict()
            .AddOpenIddictServer()

        let app = builder.Build()

        app
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseCors()

        app.MapControllers()

        app.Run()

        exitCode