namespace TimMovie.Auth

#nowarn "20"

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.OpenApi.Models
open TimMovie.Auth.AuthStartupSetup
open TimMovie.Infrastructure

module Program =
    let exitCode = 0
    let info = OpenApiInfo()
    info.Title <- "Auth server API"
    info.Version <- "v1"


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
                            .AllowAnyMethod()
                        |> ignore))
            .AddAuthenticationAndJwt()
            .AddAuthorization()
            .AddDbContext(configuration["ConnectionStrings:DefaultConnection"])

        services
            .AddIdentity()
            .ConfigureOpenIddict()
            .AddOpenIddictServer(configuration["IdentityUrl"])
            
        services.AddSwaggerGen(fun config ->
            config.SwaggerDoc("v1", info)) |> ignore

        let app = builder.Build()

        app
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseCors()
            .UseSwagger()
            .UseSwaggerUI(fun config -> config.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"))

        app.MapControllers()

        app.Run()

        exitCode