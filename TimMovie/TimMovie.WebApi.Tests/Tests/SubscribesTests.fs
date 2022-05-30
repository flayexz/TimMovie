namespace TimMovie.WebApi.Tests.Tests

open System.Net
open Newtonsoft.Json
open TimMovie.Core.DTO.Subscribes
open TimMovie.WebApi
open TimMovie.WebApi.Tests
open Xunit

type SubscribesTests(factory: BaseApplicationFactory<Program>) =
    interface IClassFixture<BaseApplicationFactory<Program>>

    [<Theory>]
    [<InlineData(true)>]
    [<InlineData(false)>]
    member this.``Test getting all user notifications``(isRequestWithJWT: bool) =
        let client = factory.CreateClient()
        let userManager = factory.GetUserManager

        if isRequestWithJWT then
            let jwtToken =
                JwtService.GetJwtToken(Constants.DefaultUserName, userManager)

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}")

        let response =
            client.GetAsync(Constants.AllUserSubscribes)
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
            let subscribes =
                JsonConvert.DeserializeObject<List<UserSubscribeDto>> result.Value

            Assert.True(
                result <> null
                && result.Succeeded
                && subscribes.Length = 1
            )
        else
            Assert.True(result <> null && result.IsFailure)

        client.Dispose()
