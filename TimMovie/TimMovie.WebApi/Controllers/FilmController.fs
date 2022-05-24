namespace TimMovie.WebApi.Controllers.ProfileController

open System
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open Microsoft.FSharp.Core
open Newtonsoft.Json
open OpenIddict.Validation.AspNetCore
open TimMovie.Core.DTO.Payment
open TimMovie.Core.Interfaces
open TimMovie.Core.Services.Films
open TimMovie.SharedKernel.Classes
open TimMovie.WebApi.Services.JwtService

[<ApiController>]
[<Route("[controller]/[action]")>]
type ProfileController
    (
        filmService: FilmService
    ) as this =
    inherit ControllerBase()

    member private _.jwtService = JwtService()

    [<HttpPost>]
    [<AllowAnonymous>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.GetFilmById([<FromForm>] filmId: Guid) =
        filmService.GetFilmById(filmId)

    [<HttpPost>]
    [<AllowAnonymous>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.GetFilmById([<FromForm>] comment: Comment) =
        filmService.TryAddCommentToFilm(comment)
