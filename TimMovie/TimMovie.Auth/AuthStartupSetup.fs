module TimMovie.Auth.AuthStartupSetup

open System
open Microsoft.AspNetCore.Authentication.JwtBearer
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Identity
open Microsoft.IdentityModel.Tokens
open OpenIddict.Abstractions
open TimMovie.Infrastructure.Database

type IServiceCollection with
    member services.AddAuthenticationAndJwt() =
        services
            .AddAuthentication(fun opt ->
                opt.DefaultAuthenticateScheme <- JwtBearerDefaults.AuthenticationScheme
                opt.DefaultChallengeScheme <- JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(fun (opt: JwtBearerOptions) -> opt.ClaimsIssuer <- JwtBearerDefaults.AuthenticationScheme)
        //                          opt.Audience <- "localhost:7097"
//                          opt.Authority <- "localhost:7282")
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
            .AddValidation(fun options ->
                options.SetIssuer("https://localhost:7282/")
                |> ignore

                options.UseSystemNetHttp() |> ignore

                options.Configure
                    (fun options ->
                        options.TokenValidationParameters.IssuerSigningKey <-
                            SymmetricSecurityKey(
                                Convert.FromBase64String("DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY=")
                            ))
                |> ignore
                
                options.UseAspNetCore() |> ignore

                options.AddEncryptionCertificate("b82f36609cdaff9a95de60e8d5ac774b2e496c4b")
                |> ignore)
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
                    .UseAspNetCore(fun builder -> builder.EnableTokenEndpointPassthrough() |> ignore)
                    .AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate()
                |> ignore)
        |> ignore

        services
