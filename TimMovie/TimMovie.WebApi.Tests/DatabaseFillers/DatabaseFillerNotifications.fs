namespace TimMovie.WebApi.Tests

open TimMovie.Core.Entities
open TimMovie.Infrastructure.Database

type DatabaseFillerNotifications() =
     member this.Start(dbContext: ApplicationContext) =
            this.Notifications(dbContext)
    
     member private this.Notifications(dbContext: ApplicationContext) =
         dbContext.Notifications.Add(Notification(Content="Notification1")) |> ignore
