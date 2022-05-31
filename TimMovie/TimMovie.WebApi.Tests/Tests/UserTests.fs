namespace TimMovie.WebApi.Tests.Tests

open System.Collections.Generic
open System.Net
open System.Net.Http
open Newtonsoft.Json
open TimMovie.Core.DTO.Notifications
open TimMovie.Core.DTO.Users
open TimMovie.WebApi
open TimMovie.WebApi.Tests
open Xunit

type UserTests(factory: BaseApplicationFactory<Program>) =
    interface IClassFixture<BaseApplicationFactory<Program>>

    [<Theory>]
    [<InlineData(true)>]
    [<InlineData(false)>]
    member this.``Test getting user information``(isRequestWithJWT: bool) =
        let client = factory.CreateClient()
        let userManager = factory.GetUserManager

        if isRequestWithJWT then
            let jwtToken =
                JwtService.GetJwtToken(Constants.DefaultUserName, userManager)

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}")

        let response =
            client.GetAsync(Constants.UserInformation)
            |> Async.AwaitTask
            |> Async.RunSynchronously

        Assert.True(response.StatusCode = HttpStatusCode.OK)

        let content =
            response.Content.ReadAsStringAsync()
            |> Async.AwaitTask
            |> Async.RunSynchronously

        let result =
            JsonConvert.DeserializeObject<TimMovie.SharedKernel.Classes.Result<string>> content

        if isRequestWithJWT then
            let user =
                JsonConvert.DeserializeObject<UserInfoDto> result.Value

            Assert.True(
                result <> null
                && result.Succeeded
                && user.DisplayName = Constants.DefaultDisplayName
                && user.PathToPhoto = Constants.DefaultPathToPhoto
            )
        else
            Assert.True(result <> null && result.IsFailure)

        client.Dispose()

    [<Theory>]
    [<InlineData(true)>]
    [<InlineData(false)>]
    member this.``Test getting user notifications``(isRequestWithJWT: bool) =
        let client = factory.CreateClient()
        let userManager = factory.GetUserManager

        if isRequestWithJWT then
            let jwtToken =
                JwtService.GetJwtToken(Constants.DefaultUserName, userManager)

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}")

        let response =
            client.GetAsync(Constants.GetAllUserNotifications)
            |> Async.AwaitTask
            |> Async.RunSynchronously

        Assert.True(response.StatusCode = HttpStatusCode.OK)

        let content =
            response.Content.ReadAsStringAsync()
            |> Async.AwaitTask
            |> Async.RunSynchronously

        let result =
            JsonConvert.DeserializeObject<TimMovie.SharedKernel.Classes.Result<string>> content

        if isRequestWithJWT then
            let notifications =
                JsonConvert.DeserializeObject<List<NotificationDto>> result.Value

            Assert.True(
                result <> null
                && result.Succeeded
                && notifications.Count = 2
            )
        else
            Assert.True(result <> null && result.IsFailure)

        client.Dispose()

    [<Theory>]
    [<InlineData(true)>]
    [<InlineData(false)>]
    member this.``Test pay for subscription``(isRequestWithJWT: bool) =
        let client = factory.CreateClient()
        let userManager = factory.GetUserManager

        let data = List<KeyValuePair<string, string>>()
        data.Add(KeyValuePair<string, string>("subscribeId", Constants.DefaultSubscribeForPaymentGuid.ToString()))
        data.Add(KeyValuePair<string, string>("cardNumber", "1234567898765432"))
        data.Add(KeyValuePair<string, string>("ccid", "123"))
        data.Add(KeyValuePair<string, string>("expirationMonth", "10"))
        data.Add(KeyValuePair<string, string>("expirationYear", "2023"))

        if isRequestWithJWT then
            let jwtToken =
                JwtService.GetJwtToken(Constants.DefaultUserName, userManager)

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}")

        let response =
            client.PostAsync(Constants.PayForSubscription, new FormUrlEncodedContent(data))
            |> Async.AwaitTask
            |> Async.RunSynchronously

        Assert.True(response.StatusCode = HttpStatusCode.OK)

        let content =
            response.Content.ReadAsStringAsync()
            |> Async.AwaitTask
            |> Async.RunSynchronously

        let result =
            JsonConvert.DeserializeObject<TimMovie.SharedKernel.Classes.Result<string>> content

        if isRequestWithJWT then
            Assert.True(
                result <> null
                && (result.Succeeded
                    || result.Error.Contains("банк отклонил вашу покупку"))
            )
        else
            Assert.True(result <> null && result.IsFailure)

        client.Dispose()
