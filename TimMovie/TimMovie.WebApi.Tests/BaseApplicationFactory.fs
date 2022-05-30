namespace TimMovie.WebApi.Tests

open System
open System.Linq
open Microsoft.AspNetCore.Authentication.JwtBearer
open Microsoft.AspNetCore.Authorization.Policy
open Microsoft.AspNetCore.Identity
open Microsoft.AspNetCore.Mvc.Testing
open Microsoft.EntityFrameworkCore
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging
open Microsoft.IdentityModel.Protocols.OpenIdConnect
open TimMovie.Core.Entities
open TimMovie.Infrastructure.Database
open TimMovie.WebApi
open TimMovie.Core

type BaseApplicationFactory<'TStartup when 'TStartup: not struct>() =
    inherit WebApplicationFactory<Program>()
    
    let mutable userManagerField = Unchecked.defaultof<UserManager<User>>
    member this.GetUserManager
        with get () = userManagerField
        and set (value) = userManagerField <- value
    
    override this.ConfigureWebHost(builder) =
        builder.ConfigureServices
            (fun (services: IServiceCollection) ->
                let descriptor =
                    Enumerable
                        .Where(services, (fun d -> d.ServiceType = typeof<DbContextOptions<ApplicationContext>>))
                        .SingleOrDefault()

                services.Remove(descriptor) |> ignore
                        
                services.AddDbContext<ApplicationContext>
                    (fun options ->
                        options.UseInMemoryDatabase("InMemoryDbForTesting")
                        |> ignore) |> ignore
                        
                let jwtBearerOptions = fun (options : JwtBearerOptions) ->
                    let config=OpenIdConnectConfiguration(Issuer=MockJwtTokens.Issuer)
                    config.SigningKeys.Add(MockJwtTokens.SecurityKey)
                    options.Configuration = config |> ignore
                    ()
                let actionJwtBearerOptions = Action<JwtBearerOptions>(jwtBearerOptions)
                services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, actionJwtBearerOptions)
                    |> ignore
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>() |> ignore
                
                let sp = services.BuildServiceProvider()
                let scope = sp.CreateScope()
                let scopedServices = scope.ServiceProvider
                let db = scopedServices.GetRequiredService<ApplicationContext>()
                let logger = scopedServices.GetRequiredService<ILogger<BaseApplicationFactory<Program>>>()
                let userManager = scopedServices.GetRequiredService<UserManager<User>>()
                this.GetUserManager <- userManager
                
                db.Database.EnsureCreated() |> ignore
                try
                    DatabaseFillerCommon.Start(db, userManager) |> ignore
                    db.SaveChanges() |> ignore
                with
                    | :? Exception as ex -> logger.LogInformation(ex.Message))
        |> ignore
        base.ConfigureWebHost(builder)

    override this.DisposeAsync() =
        let scope = this.Server.Services.CreateScope()
        let scopedServices = scope.ServiceProvider

        let db =
            scopedServices.GetRequiredService<ApplicationContext>()

        task { return! db.Database.EnsureDeletedAsync() }
        |> ignore

        base.DisposeAsync()
