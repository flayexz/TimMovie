namespace TimMovie.WebApi.Tests

open System
open System.Linq
open Microsoft.AspNetCore.Mvc.Testing
open Microsoft.EntityFrameworkCore
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging
open TimMovie.Core.Entities
open TimMovie.Infrastructure.Database

type BaseApplicationFactory<'TStartup when 'TStartup: not struct>() =
    inherit WebApplicationFactory<Program>()

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
                        |> ignore)
                |> ignore
                let sp = services.BuildServiceProvider()
                let scope = sp.CreateScope()
                let scopedServices = scope.ServiceProvider
                let db = scopedServices.GetRequiredService<ApplicationContext>()
                let logger = scopedServices.GetRequiredService<ILogger<BaseApplicationFactory<Program>>>()
                
                db.Database.EnsureCreated() |> ignore
                try
                    db.Genres.Add(Genre(Name = "Жанр")) |> ignore
                    db.SaveChanges() |> ignore
                with
                    | :? Exception as ex -> logger.LogInformation(ex.Message)
                scope.Dispose())
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
