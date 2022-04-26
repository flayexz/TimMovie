module TimMovie.Auth.AuthStartupSetup

open System
open Microsoft.AspNetCore.Authentication.JwtBearer
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Identity
open OpenIddict.Abstractions
open TimMovie.Infrastructure.Database

type IServiceCollection with
    member services.AddAuthenticationAndJwt() =
        services
            .AddAuthentication(fun opt ->
                opt.DefaultAuthenticateScheme <- JwtBearerDefaults.AuthenticationScheme
                opt.DefaultChallengeScheme <- JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(fun (opt: JwtBearerOptions) ->
                            opt.ClaimsIssuer <- JwtBearerDefaults.AuthenticationScheme)
        |> ignore

        services

    member services.ConfigureOpenIddict() =
        services.Configure<IdentityOptions>
            (fun (options: IdentityOptions) ->
                options.ClaimsIdentity.UserNameClaimType <- OpenIddictConstants.Claims.Name
                options.ClaimsIdentity.UserIdClaimType <- OpenIddictConstants.Claims.Subject
                options.ClaimsIdentity.RoleClaimType <- OpenIddictConstants.Claims.Role
                options.ClaimsIdentity.EmailClaimType <- OpenIddictConstants.Claims.Email)
        |> ignore

        services

    member services.AddOpenIddictServer() =
        services
            .AddOpenIddict()
            .AddCore(fun options ->
                options
                    .UseEntityFrameworkCore()
                    .UseDbContext<ApplicationContext>()
                |> ignore)
            .AddServer(fun options ->          
                options
                    .AcceptAnonymousClients()
                    .AllowPasswordFlow()
                    .AllowRefreshTokenFlow()
                    .SetAccessTokenLifetime(TimeSpan.FromDays(365))
                    .SetTokenEndpointUris("/connect/token")
                    .SetIssuer(Uri("https://localhost:7282"))
                    .AddEphemeralEncryptionKey()
                    .AddEphemeralSigningKey()
                    //.AddDevelopmentEncryptionCertificate()
                    //.AddDevelopmentSigningCertificate()
                    .DisableAccessTokenEncryption() |> ignore   // если хочешь, чтобы на jwt.io были зашифрованные данные, то убрать (их все равно расшифровать нет проблем если что)
                options
                    .UseAspNetCore()
                    .DisableTransportSecurityRequirement()
                    .EnableTokenEndpointPassthrough()
                |> ignore)
            .AddValidation(fun options ->
                options.UseSystemNetHttp() |> ignore 
                options.UseAspNetCore() |> ignore
                options.UseLocalServer() |> ignore
            )
        |> ignore

        services
