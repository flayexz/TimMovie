namespace TimMovie.WebApi.Tests.Tests

open System.Collections.Generic
open System.Net
open System.Net.Http
open TimMovie.WebApi
open TimMovie.WebApi.Tests
open Xunit

type RegistrationTests(factory: BaseApplicationFactory<Program>) =
    interface IClassFixture<BaseApplicationFactory<Program>>
    
    [<Theory>]
    [<InlineData(true)>]
    member this.``Test registration``(isEmailConfirming: bool) =
         let client = factory.CreateClient()
         let data = List<KeyValuePair<string, string>>()
         data.Add(KeyValuePair<string, string>("email", Constants.DefaultEmail))
         data.Add(KeyValuePair<string, string>("username", Constants.DefaultUserName))
         data.Add(KeyValuePair<string, string>("password", Constants.DefaultPassword))
//         task {
//            let! response = client.PostAsync(Constants.Registration, new FormUrlEncodedContent(data))
//            Assert.True(response.StatusCode = HttpStatusCode.OK)
//         }
         let response = client.PostAsync(Constants.Registration, new FormUrlEncodedContent(data))
                        |> Async.AwaitTask
                        |> Async.RunSynchronously
         Assert.True(response.StatusCode = HttpStatusCode.OK)
         let content = response.Content.ReadAsStringAsync()
                        |> Async.AwaitTask
                        |> Async.RunSynchronously
         if isEmailConfirming then
            let response = client.GetAsync(Constants.UrlToConfirmEmail)
                           |> Async.AwaitTask
                           |> Async.RunSynchronously
            Assert.True(response.StatusCode = HttpStatusCode.OK)
            let content = response.Content.ReadAsStringAsync()
                        |> Async.AwaitTask
                        |> Async.RunSynchronously
            ignore |> ignore 
         client.Dispose()