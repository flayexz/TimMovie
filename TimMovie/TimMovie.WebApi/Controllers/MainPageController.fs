namespace TimMovie.WebApi.Controllers.AuthorizationController

open System
open System.Text.Json
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open Microsoft.FSharp.Core
open Newtonsoft.Json
open OpenIddict.Validation.AspNetCore
open TimMovie.Core.Interfaces
open TimMovie.SharedKernel.Classes
open TimMovie.WebApi.Services.JwtService

[<ApiController>]
[<Route("[controller]/[action]")>]
type MainPageController
    (
        searchEntityService: ISearchEntityService,
        subscribeService: ISubscribeService,
        notificationService: INotificationService
    ) as this =
    inherit ControllerBase()

    member private _.jwtService = JwtService()

    [<HttpPost>]
    [<Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.SearchEntityByNamePart([<FromForm>] namePart: string) =
        searchEntityService.GetSearchEntityResultByNamePart(namePart)

    [<HttpPost>]
    [<Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.GetUserSubscribes([<FromForm>] userGuid: Guid) =
        subscribeService.GetAllActiveUserSubscribes(userGuid)

    [<HttpPost>]
    [<Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.GetAllUserNotifications([<FromForm>] userGuid: Guid) =
        let jwtTokenOption =
            this.jwtService.GetUserJwtToken(this.HttpContext.Request.Headers)

        if jwtTokenOption.IsSome then
            let userGuidOption =
                this.jwtService.GetUserGuid(jwtTokenOption.Value)

            if userGuidOption.IsSome then
                if (userGuidOption.Value <> userGuid.ToString()) then
                    Result.Fail<string>("Access denied")
                else
                    let notifications =
                        notificationService.GetAllUserNotifications(userGuid)

                    let json =
                        JsonConvert.SerializeObject notifications

                    Result.Ok(json)
            else
                Result.Fail<string>("Error occurred while decoding the jwt token")
        else
            Result.Fail<string>("Error occurred while getting user jwt token")

//    //TODO: доделать сериализацию
//    [<HttpPost>]
//    [<Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)>]
//    [<Consumes("application/x-www-form-urlencoded")>]
//    member _.GetAllSubscribes([<FromForm>] namePart: string) =
//        let subscribes =
//            subscribeService.GetSubscribesByNamePart(namePart)
//
//        JsonSerializer.Serialize subscribes
//
//    //TODO: доделать сериализацию
//    [<HttpPost>]
//    [<Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)>]
//    [<Consumes("application/x-www-form-urlencoded")>]
//    member _.GetAllSubscribesWithPagination
//        (
//            [<FromForm>] namePart: string,
//            [<FromForm>] take: int,
//            [<FromForm>] skip: int
//        ) =
//        let subscribes =
//            subscribeService.GetSubscribesByNamePart(namePart, take, skip)
//
//        JsonSerializer.Serialize subscribes
