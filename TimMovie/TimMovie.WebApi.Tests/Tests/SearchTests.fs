namespace TimMovie.WebApi.Tests.Tests

open System.Net.Http
open Microsoft.AspNetCore.Http
open Newtonsoft.Json
open TimMovie.WebApi.Tests
open Xunit
open TimMovie.WebApi

type SearchTests(factory: BaseApplicationFactory<Program>) =
    interface IClassFixture<BaseApplicationFactory<Program>>
    
    [<Fact>]
     member this.``[TEST] [WITH JWT]Получить единственный фильм``() =
        let route = RouteConstants.NavbarSearch
        let client = factory.CreateClient()
        let body = {| namePart  = "F1" |}
        let httpMessage = client.GetAsync(route) |> Async.AwaitTask |> Async.RunSynchronously
        Assert.True(httpMessage.StatusCode = System.Net.HttpStatusCode.OK)
//        task {
////            let! httpMessage = client.PostAsync(route, new StringContent(JsonConvert.SerializeObject body))
//            let! httpMessage = client.GetAsync(route)
//            Assert.True(httpMessage.StatusCode = System.Net.HttpStatusCode.OK)
//        }
