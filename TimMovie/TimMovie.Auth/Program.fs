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

    let scheme = OpenApiSecurityScheme()
    scheme.Name <- "Authorization"
    scheme.Type <- SecuritySchemeType.ApiKey
    scheme.Scheme <- "Bearer"
    scheme.BearerFormat <- "JWT"
    scheme.In <- ParameterLocation.Header

    scheme.Description <-
        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\""

    let securityRequirement = OpenApiSecurityRequirement()
    let securityScheme = OpenApiSecurityScheme()
    let reference = OpenApiReference()
    reference.Type <- ReferenceType.SecurityScheme
    reference.Id <- "Bearer"
    securityScheme.Reference <- reference
    securityRequirement.Add(securityScheme, Array.empty)

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

        services.AddSwaggerGen
            (fun config ->
                config.SwaggerDoc("v1", info)
                config.AddSecurityDefinition("Bearer", scheme)
                config.AddSecurityRequirement(securityRequirement))
        |> ignore

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
