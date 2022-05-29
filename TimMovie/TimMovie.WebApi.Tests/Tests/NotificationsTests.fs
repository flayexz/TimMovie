namespace TimMovie.WebApi.Tests.Tests

open System.Collections.Generic
open System.Net
open System.Net.Http
open Microsoft.VisualBasic
open Newtonsoft.Json
open TimMovie.Core.DTO
open TimMovie.WebApi.Services.JwtService
open TimMovie.WebApi.Tests
open Xunit
open TimMovie.WebApi

type NotificationsTests(factory: BaseApplicationFactory<Program>) =
    interface IClassFixture<BaseApplicationFactory<Program>>

    [<Theory>]
    [<InlineData(true)>]
    member this.``Test search``(isRequestWithJWT: bool) =
        let client = factory.CreateClient()
        if isRequestWithJWT then
            let jwtService = JwtService()
            let jwtToken =
                jwtService.Authorize(Constants.Authorization, Constants.DefaultUserName, Constants.DefaultPassword)
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}")
        task {
            let! response = client.GetAsync(Constants.Notifications)
            client.Dispose()
            if isRequestWithJWT then
                Assert.True(response.StatusCode = HttpStatusCode.OK)
                let! content = response.Content.ReadAsStringAsync()
                let result =
                    JsonConvert.DeserializeObject<List<Notifications.NotificationDto>> content
                Assert.True(
                    result <> null &&
                    result.Count <> 0
                )
            else
                Assert.True(response.StatusCode = HttpStatusCode.Unauthorized)
        }
