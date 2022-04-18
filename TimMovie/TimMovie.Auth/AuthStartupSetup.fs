module TimMovie.Auth.AuthStartupSetup

open System
open Microsoft.AspNetCore.Authentication.JwtBearer
open Microsoft.Extensions.DependencyInjection
open TimMovie.Core.Entities
open Microsoft.AspNetCore.Identity
open OpenIddict.Abstractions
open TimMovie.Infrastructure.Database
open TimMovie.Infrastructure.Identity;

type IServiceCollection with
    member services.AddAuthenticationWithJwt() =
            services
                .AddAuthentication(fun opt ->
                    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme
                        |> ignore
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme
                        |> ignore)
                .AddJwtBearer(fun opt -> opt.ClaimsIssuer = JwtBearerDefaults.AuthenticationScheme
                                         |> ignore)
            |> ignore
            services
           
    member services.AddIdentity() =
            services
                .AddIdentity<User,IdentityRole<Guid>>(fun opt ->
                        opt.Password.RequireUppercase = false
                            |> ignore
                        opt.Password.RequireDigit = true
                            |> ignore
                        opt.Password.RequireNonAlphanumeric = false
                            |> ignore
                        opt.Password.RequiredLength = 8
                            |> ignore
                        opt.User.RequireUniqueEmail = true
                            |> ignore
                        opt.SignIn.RequireConfirmedEmail = true
                            |> ignore)
                .AddErrorDescriber<RussianErrorDescriber>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders()
            |> ignore
            
            services.Configure<IdentityOptions>(fun (options:IdentityOptions) ->
                options.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Name |> ignore
                options.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject |> ignore
                options.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role |> ignore
                options.ClaimsIdentity.EmailClaimType = OpenIddictConstants.Claims.Email |> ignore)
            |> ignore
            
            services
            
    member services.AddOpenIdConnect() =
            services
                .AddOpenIddict()
                .AddCore(fun options ->
                    options
                        .UseEntityFrameworkCore()
                        .UseDbContext<ApplicationContext>()
                    |> ignore)
                .AddServer(fun options ->
                    options
                        .SetAccessTokenLifetime(TimeSpan.FromDays(365))
                        .AcceptAnonymousClients()
                        .AllowPasswordFlow()
                        .AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate()
                        .AllowRefreshTokenFlow()
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
            
            