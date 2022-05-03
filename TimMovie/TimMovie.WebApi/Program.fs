namespace TimMovie.WebApi

#nowarn "20"

open Autofac
open Autofac.Extensions.DependencyInjection
open Microsoft.OpenApi.Models
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open OpenIddict.Validation.AspNetCore
open TimMovie.Core
open TimMovie.Infrastructure

module Program =
    let exitCode = 0

    let info = OpenApiInfo()
    info.Title <- "WebAPI server"
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


        builder.Host.UseServiceProviderFactory(AutofacServiceProviderFactory())
        builder.Services.Configure(builder.Configuration)
        builder.Host.ConfigureContainer<ContainerBuilder>
            (fun (containerBuilder: ContainerBuilder) ->
                containerBuilder.RegisterModule(InfrastructureModule(builder.Configuration))
                |> ignore)

        let services = builder.Services
        let configuration = builder.Configuration

        services.AddControllers()

        services.AddCors
            (fun options ->
                options.AddDefaultPolicy
                    (fun builder ->
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                        |> ignore))

        services.AddAuthentication
            (fun options -> options.DefaultScheme <- OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)

        services.AddAuthorization()

        services
            .AddOpenIddict()
            .AddValidation(fun options ->
                options.SetIssuer(configuration["IdentityUrl"])
                |> ignore

                options.UseSystemNetHttp() |> ignore
                options.UseAspNetCore() |> ignore)

        //            config.AddSecurityDefinition("v1", info)
//            config.OperationFilter<GetTokenFilter>()) |> ignore
        services.AddSwaggerGen
            (fun config ->
                //            config.SwaggerDoc("v1", info)
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
