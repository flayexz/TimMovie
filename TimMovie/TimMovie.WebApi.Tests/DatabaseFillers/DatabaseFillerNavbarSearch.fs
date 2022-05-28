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
         dbContext.Films.Add(Film(Title="F1A0P0G0")) |> ignore
         dbContext.Films.Add(Film(Title="F2A0P0G0")) |> ignore
         dbContext.Films.Add(Film(Title="F2A0P0G0")) |> ignore
         dbContext.Films.Add(Film(Title="F3A0P0G0")) |> ignore
         dbContext.Films.Add(Film(Title="F3A0P0G0")) |> ignore
         dbContext.Films.Add(Film(Title="F3A0P0G0")) |> ignore
         dbContext.Films.Add(Film(Title="F4A0P0G0")) |> ignore
         dbContext.Films.Add(Film(Title="F4A0P0G0")) |> ignore
         dbContext.Films.Add(Film(Title="F4A0P0G0")) |> ignore
         dbContext.Films.Add(Film(Title="F4A0P0G0")) |> ignore
         dbContext.Films.Add(Film(Title="F5A0P0G0")) |> ignore
         dbContext.Films.Add(Film(Title="F5A0P0G0")) |> ignore
         dbContext.Films.Add(Film(Title="F5A0P0G0")) |> ignore
         dbContext.Films.Add(Film(Title="F5A0P0G0")) |> ignore
         dbContext.Films.Add(Film(Title="F5A0P0G0")) |> ignore
         dbContext.Films.Add(Film(Title="F1A1P0G0")) |> ignore
         dbContext.Films.Add(Film(Title="F1A1P1G1")) |> ignore
         dbContext.Films.Add(Film(Title="F3A3P3G3")) |> ignore
         dbContext.Films.Add(Film(Title="F5A3P3G3")) |> ignore
        
     member private this.AddActors(dbContext: ApplicationContext) =
         dbContext.Actors.Add(Actor(Name="actorName", Surname="actorSurname")) |> ignore
         dbContext.Actors.Add(Actor(Name="F0A1P0G0")) |> ignore
         dbContext.Actors.Add(Actor(Name="F0A2P0G0")) |> ignore
         dbContext.Actors.Add(Actor(Name="F0A2P0G0")) |> ignore
         dbContext.Actors.Add(Actor(Name="F0A3P0G0")) |> ignore
         dbContext.Actors.Add(Actor(Name="F0A3P0G0")) |> ignore
         dbContext.Actors.Add(Actor(Name="F0A3P0G0")) |> ignore
         dbContext.Actors.Add(Actor(Name="F1A1P0G0")) |> ignore
         dbContext.Actors.Add(Actor(Name="F1A1P1G1")) |> ignore
         dbContext.Actors.Add(Actor(Name="F3A3P3G3")) |> ignore
         dbContext.Actors.Add(Actor(Name="F5A3P3G3")) |> ignore
     
     
     member private this.AddProducers(dbContext: ApplicationContext) =
         dbContext.Producers.Add(Producer(Name="producerName", Surname="producerSurname")) |> ignore
         dbContext.Producers.Add(Producer(Name="F0A0P1G0")) |> ignore
         dbContext.Producers.Add(Producer(Name="F0A0P2G0")) |> ignore
         dbContext.Producers.Add(Producer(Name="F0A0P2G0")) |> ignore
         dbContext.Producers.Add(Producer(Name="F0A0P3G0")) |> ignore
         dbContext.Producers.Add(Producer(Name="F0A0P3G0")) |> ignore
         dbContext.Producers.Add(Producer(Name="F0A0P3G0")) |> ignore
         dbContext.Producers.Add(Producer(Name="F1A1P1G1")) |> ignore
         dbContext.Producers.Add(Producer(Name="F3A3P3G3")) |> ignore
         dbContext.Producers.Add(Producer(Name="F5A3P3G3")) |> ignore
         
     member private this.AddGenres(dbContext: ApplicationContext) =
         dbContext.Genres.Add(Genre(Name="F0A0P0G1")) |> ignore
         dbContext.Genres.Add(Genre(Name="F0A0P0G2")) |> ignore
         dbContext.Genres.Add(Genre(Name="F0A0P0G2")) |> ignore
         dbContext.Genres.Add(Genre(Name="F0A0P0G3")) |> ignore
         dbContext.Genres.Add(Genre(Name="F0A0P0G3")) |> ignore
         dbContext.Genres.Add(Genre(Name="F0A0P0G3")) |> ignore
         dbContext.Genres.Add(Genre(Name="F1A1P1G1")) |> ignore
         dbContext.Genres.Add(Genre(Name="F3A3P3G3")) |> ignore
         dbContext.Genres.Add(Genre(Name="F5A3P3G3")) |> ignore
    