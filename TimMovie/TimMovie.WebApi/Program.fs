namespace TimMovie.WebApi

#nowarn "20"

open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open OpenIddict.Validation.AspNetCore

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
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                        |> ignore))
       
        services.AddAuthentication(fun options ->
            options.DefaultScheme <- OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
        
        services.AddAuthorization()
        
        services.AddOpenIddict()
            .AddValidation(fun options ->
                options.SetIssuer(configuration["IdentityUrl"]) |> ignore
                options.UseSystemNetHttp() |> ignore
                options.UseAspNetCore() |> ignore) 
     
        let app = builder.Build()

        app
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseCors()

        app.MapControllers()

        app.Run()

        exitCode