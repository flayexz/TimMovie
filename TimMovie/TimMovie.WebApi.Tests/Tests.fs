namespace TimMovie.WebApi.Tests.Tests

open TimMovie.Infrastructure.Database
open TimMovie.WebApi.Tests
open Xunit

type UpdateTests(factory: BaseApplicationFactory<Program>) =
    interface IClassFixture<BaseApplicationFactory<Program>>
    
    [<Fact>]
    member this.``My test``() =
        let client = factory.CreateClient()
        let genres = appContext.Genres |> Seq.toList
        Assert.True(true)