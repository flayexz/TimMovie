namespace TimMovie.WebApi.Tests.Tests

open System.Net
open Newtonsoft.Json
open TimMovie.Core.DTO.Notifications
open TimMovie.WebApi.Tests
open Xunit
open TimMovie.WebApi

type NotificationsTests(factory: BaseApplicationFactory<Program>) =
    interface IClassFixture<BaseApplicationFactory<Program>>

    [<Theory>]
    [<InlineData(true)>]
    member this.``Test search``(isRequestWithJWT: bool) =
        let client = factory.CreateClient()
        let userManager = factory.GetUserManager
        if isRequestWithJWT then
            let jwtToken =
                JwtService.GetJwtToken(Constants.DefaultUserName, userManager)
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}")
        let response =
            client.GetAsync(Constants.Notifications)
            |> Async.AwaitTask
            |> Async.RunSynchronously
        let content =
            response.Content.ReadAsStringAsync()
            |> Async.AwaitTask
            |> Async.RunSynchronously
        let result =
            JsonConvert.DeserializeObject<TimMovie.SharedKernel.Classes.Result<string>>
                content
        let notifications =
            JsonConvert.DeserializeObject<List<NotificationDto>> result.Value
        Assert.True(
            response.StatusCode = HttpStatusCode.OK
            && result <> null
            && result.Succeeded
            && notifications.Length = 1)
        Assert.False(notifications.Length = 0
                     && notifications.Length = 2)
        
        client.Dispose()
