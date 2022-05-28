namespace TimMovie.WebApi.Tests

open TimMovie.Core.Entities
open TimMovie.Infrastructure.Database

type DatabaseFillerNavbarSearch() =
     member this.Start(dbContext: ApplicationContext) =
            this.AddFilms(dbContext)
            this.AddActors(dbContext)
            this.AddProducers(dbContext)
            this.AddGenres(dbContext)
    
     member private this.AddFilms(dbContext: ApplicationContext) =
         dbContext.Films.Add(Film(Title="F1")) |> ignore
         dbContext.Films.Add(Film(Title="F2")) |> ignore
         dbContext.Films.Add(Film(Title="F2")) |> ignore
         dbContext.Films.Add(Film(Title="F3")) |> ignore
         dbContext.Films.Add(Film(Title="F3")) |> ignore
         dbContext.Films.Add(Film(Title="F3")) |> ignore
         dbContext.Films.Add(Film(Title="F4")) |> ignore
         dbContext.Films.Add(Film(Title="F4")) |> ignore
         dbContext.Films.Add(Film(Title="F4")) |> ignore
         dbContext.Films.Add(Film(Title="F4")) |> ignore
         dbContext.Films.Add(Film(Title="F5")) |> ignore
         dbContext.Films.Add(Film(Title="F5")) |> ignore
         dbContext.Films.Add(Film(Title="F5")) |> ignore
         dbContext.Films.Add(Film(Title="F5")) |> ignore
         dbContext.Films.Add(Film(Title="F5")) |> ignore
         dbContext.Films.Add(Film(Title="F1A1")) |> ignore
         dbContext.Films.Add(Film(Title="F1A1P1G1")) |> ignore
         dbContext.Films.Add(Film(Title="F3A3P3G3")) |> ignore
         dbContext.Films.Add(Film(Title="F5A3P3G3")) |> ignore
        
     member private this.AddActors(dbContext: ApplicationContext) =
         dbContext.Actors.Add(Actor(Name="actorName", Surname="actorSurname")) |> ignore
         dbContext.Actors.Add(Actor(Name="A1")) |> ignore
         dbContext.Actors.Add(Actor(Name="A2")) |> ignore
         dbContext.Actors.Add(Actor(Name="A2")) |> ignore
         dbContext.Actors.Add(Actor(Name="A3")) |> ignore
         dbContext.Actors.Add(Actor(Name="A3")) |> ignore
         dbContext.Actors.Add(Actor(Name="A3")) |> ignore
         dbContext.Actors.Add(Actor(Name="F1A1")) |> ignore
         dbContext.Actors.Add(Actor(Name="F1A1P1G1")) |> ignore
         dbContext.Actors.Add(Actor(Name="F3A3P3G3")) |> ignore
         dbContext.Actors.Add(Actor(Name="F5A3P3G3")) |> ignore
     
     
     member private this.AddProducers(dbContext: ApplicationContext) =
         dbContext.Producers.Add(Producer(Name="producerName", Surname="producerSurname")) |> ignore
         dbContext.Producers.Add(Producer(Name="P1")) |> ignore
         dbContext.Producers.Add(Producer(Name="P2")) |> ignore
         dbContext.Producers.Add(Producer(Name="P2")) |> ignore
         dbContext.Producers.Add(Producer(Name="P3")) |> ignore
         dbContext.Producers.Add(Producer(Name="P3")) |> ignore
         dbContext.Producers.Add(Producer(Name="P3")) |> ignore
         dbContext.Producers.Add(Producer(Name="F1A1P1G1")) |> ignore
         dbContext.Producers.Add(Producer(Name="F3A3P3G3")) |> ignore
         dbContext.Producers.Add(Producer(Name="F5A3P3G3")) |> ignore
         
     member private this.AddGenres(dbContext: ApplicationContext) =
         dbContext.Genres.Add(Genre(Name="G1")) |> ignore
         dbContext.Genres.Add(Genre(Name="G2")) |> ignore
         dbContext.Genres.Add(Genre(Name="G2")) |> ignore
         dbContext.Genres.Add(Genre(Name="G3")) |> ignore
         dbContext.Genres.Add(Genre(Name="G3")) |> ignore
         dbContext.Genres.Add(Genre(Name="G3")) |> ignore
         dbContext.Genres.Add(Genre(Name="F1A1P1G1")) |> ignore
         dbContext.Genres.Add(Genre(Name="F3A3P3G3")) |> ignore
         dbContext.Genres.Add(Genre(Name="F5A3P3G3")) |> ignore
    