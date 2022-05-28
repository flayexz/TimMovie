namespace TimMovie.WebApi.Tests.Tests

open System.Collections.Generic
open System.Net
open System.Net.Http
open Newtonsoft.Json
open TimMovie.Core.DTO
open TimMovie.WebApi.Tests
open Xunit
open TimMovie.WebApi

type SearchTests(factory: BaseApplicationFactory<Program>) =
    interface IClassFixture<BaseApplicationFactory<Program>>
    
    [<Fact>]
     member this.``[TEST] [WITH JWT]Получить единственный фильм``() =
        let route = RouteConstants.NavbarSearch
        let client = factory.CreateClient()
        let data = List<KeyValuePair<string, string>>();
        data.Add(KeyValuePair<string, string>("namePart", "F1A0P0G0"));
        task {
            let! response = client.PostAsync(route, new FormUrlEncodedContent(data))
            Assert.True(response.StatusCode = HttpStatusCode.OK)
            let! content = response.Content.ReadAsStringAsync()
            let result = JsonConvert.DeserializeObject<SearchEntityResultDto> content
            Assert.True(result <> null &&
                        result <> SearchEntityResultDto() &&
                        result.Films <> null &&
                        result.Films |> Seq.length = 1)
        }
