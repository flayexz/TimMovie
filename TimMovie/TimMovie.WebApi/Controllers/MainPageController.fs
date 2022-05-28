namespace TimMovie.WebApi.Controllers.MainPageController

open System
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open Microsoft.FSharp.Core
open Newtonsoft.Json
open OpenIddict.Validation.AspNetCore
open TimMovie.Core.DTO
open TimMovie.Core.DTO.Films
open TimMovie.Core.Interfaces
open TimMovie.Core.Services.Films
open TimMovie.SharedKernel.Classes
open TimMovie.WebApi.Services.JwtService

[<ApiController>]
[<Route("[controller]/[action]")>]
type MainPageController
    (
        searchEntityService: ISearchEntityService,
        subscribeService: ISubscribeService,
        notificationService: INotificationService,
        filmCardService: FilmCardService
    ) as this =
    inherit ControllerBase()

    member private _.jwtService = JwtService()

    [<HttpGet>]
//    [<Consumes("application/x-www-form-urlencoded")>]
    member _.Get123() =
        "123"
        
    [<HttpPost>]
    [<AllowAnonymous>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.SearchEntityByNamePart([<FromForm>] namePart: string) =
        searchEntityService.GetSearchEntityResultByNamePart(namePart)

    [<HttpGet>]
    [<Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.GetAllUserNotifications() =
        let jwtTokenOption =
            this.jwtService.GetUserJwtToken(this.HttpContext.Request.Headers)

        if jwtTokenOption.IsSome then
            let userGuidOption =
                this.jwtService.GetUserGuid(jwtTokenOption.Value)

            if userGuidOption.IsSome then
                let notifications =
                    notificationService.GetAllUserNotifications(Guid(userGuidOption.Value.ToString()))

                let json =
                    JsonConvert.SerializeObject notifications

                Result.Ok(json)
            else
                Result.Fail<string>("Error occurred while decoding the jwt token")
        else
            Result.Fail<string>("Error occurred while getting user jwt token")
            
    [<HttpGet>]
    [<Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.GetUserSubscribes() =
        let jwtTokenOption =
            this.jwtService.GetUserJwtToken(this.HttpContext.Request.Headers)

        if jwtTokenOption.IsSome then
            let userGuidOption =
                this.jwtService.GetUserGuid(jwtTokenOption.Value)

            if userGuidOption.IsSome then
                    let subscribes =
                        subscribeService.GetAllActiveUserSubscribes(Guid(userGuidOption.Value.ToString()))

                    let json =
                        JsonConvert.SerializeObject subscribes

                    Result.Ok(json)
            else
                Result.Fail<string>("Error occurred while decoding the jwt token")
        else
            Result.Fail<string>("Error occurred while getting user jwt token")

    [<HttpPost>]
    [<AllowAnonymous>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.GetAllSubscribesByNamePart([<FromForm>] namePart: string) =
        subscribeService.GetSubscribesByNamePart(namePart)

    [<HttpPost>]
    [<AllowAnonymous>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.GetSubscribesByNamePartWithPagination
        (
            [<FromForm>] namePart: string,
            [<FromForm>] take: int,
            [<FromForm>] skip: int
        ) =
        subscribeService.GetSubscribesByNamePart(namePart, take, skip)
        
    [<HttpPost>]
    [<AllowAnonymous>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.GetFilmByFilters([<FromForm>] generalPaginationDto: GeneralPaginationDto<SelectedFilmFiltersDto>) =
        filmCardService.GetFilmCardsByFilters(generalPaginationDto)
