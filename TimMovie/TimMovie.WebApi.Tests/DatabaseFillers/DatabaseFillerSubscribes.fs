namespace TimMovie.WebApi.Tests

open System
open Microsoft.AspNetCore.Identity
open TimMovie.Core.Entities
open TimMovie.Infrastructure.Database

type DatabaseFillerSubscribes() =
    member this.Start(dbContext: ApplicationContext, userManager: UserManager<User>) =
        this.AddSubscribes(dbContext, userManager)

    member private this.AddSubscribes(dbContext: ApplicationContext, userManager: UserManager<User>) =
        let user =
            userManager.FindByNameAsync(Constants.DefaultUserName)
            |> Async.AwaitTask
            |> Async.RunSynchronously

        dbContext.Subscribes.Add(Subscribe(Name = "TestSubscribe", Price = 1))
        |> ignore

        dbContext.UserSubscribes.Add(
            UserSubscribe(
                SubscribedUser = user,
                Subscribe = Subscribe(Name = "TestSubscribe", Price = 1),
                StartDay = DateTime.Now,
                EndDate = DateTime.Today.AddDays(1)
            )
        )
        |> ignore
