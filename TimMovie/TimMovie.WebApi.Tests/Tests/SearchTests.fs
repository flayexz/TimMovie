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

    [<Theory>]
    [<InlineData("F1A0P0G0", 1, 0, 0, 0)>]
    [<InlineData("F2A0P0G0", 2, 0, 0, 0)>]
    [<InlineData("F3A0P0G0", 3, 0, 0, 0)>]
    [<InlineData("F4A0P0G0", 4, 0, 0, 0)>]
    [<InlineData("F5A0P0G0", 4, 0, 0, 0)>]
    [<InlineData("F1A1P0G0", 1, 1, 0, 0)>]
    [<InlineData("F1A1P0G0", 1, 1, 0, 0)>]
    [<InlineData("F1A1P1G1", 1, 1, 1, 1)>]
    [<InlineData("F3A3P3G3", 3, 2, 2, 2)>]
    [<InlineData("F5A3P3G3", 4, 2, 2, 2)>]
    [<InlineData("F0A1P0G0", 0, 1, 0, 0)>]
    [<InlineData("F0A2P0G0", 0, 2, 0, 0)>]
    [<InlineData("F0A3P0G0", 0, 2, 0, 0)>]
    [<InlineData("F0A0P1G0", 0, 0, 1, 0)>]
    [<InlineData("F0A0P2G0", 0, 0, 2, 0)>]
    [<InlineData("F0A0P3G0", 0, 0, 2, 0)>]
    [<InlineData("F0A0P0G1", 0, 0, 0, 1)>]
    [<InlineData("F0A0P0G2", 0, 0, 0, 2)>]
    [<InlineData("F0A0P0G3", 0, 0, 0, 2)>]
    [<InlineData("F1", 3, 2, 1, 1)>]
    [<InlineData("Фильм", 2, 0, 0, 0)>]
    [<InlineData("фильм", 2, 0, 0, 0)>]
    [<InlineData("Film", 2, 0, 0, 0)>]
    [<InlineData("film", 2, 0, 0, 0)>]
    [<InlineData("Имя Фамилия", 0, 2, 2, 0)>]
    [<InlineData("имя Фамилия", 0, 2, 2, 0)>]
    [<InlineData("Имя фамилия", 0, 2, 2, 0)>]
    [<InlineData("имя фамилия", 0, 2, 2, 0)>]
    [<InlineData("имяфамилия", 0, 0, 0, 0)>]
    [<InlineData("Name Surname", 0, 2, 2, 0)>]
    [<InlineData("name Surname", 0, 2, 2, 0)>]
    [<InlineData("Name surname", 0, 2, 2, 0)>]
    [<InlineData("name surname", 0, 2, 2, 0)>]
    [<InlineData("namesurname", 0, 0, 0, 0)>]
    [<InlineData("Жанр", 0, 0, 0, 2)>]
    [<InlineData("жанр", 0, 0, 0, 2)>]
    [<InlineData("Genre", 0, 0, 0, 2)>]
    [<InlineData("genre", 0, 0, 0, 2)>]
    member this.``Test search``
        (
            value: string,
            filmsCount: int,
            actorsCount: int,
            producersCount: int,
            genresCount: int
        ) =
        let client = factory.CreateClient()
        let data = List<KeyValuePair<string, string>>()
        data.Add(KeyValuePair<string, string>("namePart", value))
        task {
            let! response = client.PostAsync(Constants.NavbarSearch, new FormUrlEncodedContent(data))
            client.Dispose()
            Assert.True(response.StatusCode = HttpStatusCode.OK)
            let! content = response.Content.ReadAsStringAsync()
            let result =
                JsonConvert.DeserializeObject<SearchEntityResultDto> content
            Assert.True(
                result <> null
                && result <> SearchEntityResultDto()
                && result.Films <> null
                && result.Films |> Seq.length = filmsCount
                && result.Actors <> null
                && result.Actors |> Seq.length = actorsCount
                && result.Producers <> null
                && result.Producers |> Seq.length = producersCount
                && result.Genres <> null
                && result.Genres |> Seq.length = genresCount
            )
        }
        
