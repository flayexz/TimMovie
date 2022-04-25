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
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme
                        |> ignore
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme
                        |> ignore)
                .AddJwtBearer(fun opt ->
                    opt.ClaimsIssuer = JwtBearerDefaults.AuthenticationScheme
                        |> ignore)
            |> ignore
            
            services
            
    member services.ConfigureOpenIddict() =      
        services.Configure<IdentityOptions>(fun (options:IdentityOptions) ->
            options.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Name
                |> ignore
            options.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject
                |> ignore
            options.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role
                |> ignore
            options.ClaimsIdentity.EmailClaimType = OpenIddictConstants.Claims.Email
                |> ignore)
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
                        .UseAspNetCore(fun builder ->
                            builder
                                .EnableTokenEndpointPassthrough()
                            |> ignore)
                        .AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate()
                    |> ignore)
                .AddValidation(fun options ->
                    options.UseAspNetCore() |> ignore
                    options.UseLocalServer() |> ignore)
            |> ignore
        
            services