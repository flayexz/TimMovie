namespace TimMovie.WebApi.Controllers.ProfileController

open System
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open Microsoft.FSharp.Core
open Newtonsoft.Json
open OpenIddict.Validation.AspNetCore
open TimMovie.Core.DTO.Payment
open TimMovie.Core.Interfaces
open TimMovie.SharedKernel.Classes
open TimMovie.WebApi.Services.JwtService

[<ApiController>]
[<Route("[controller]/[action]")>]
type ProfileController
    (
        userService: IUserService,
        notificationService: INotificationService,
        paymentService: IPaymentService
    ) as this =
    inherit ControllerBase()

    member private _.jwtService = JwtService()

    [<HttpGet>]
    [<Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.GetUserInformation() =
        let jwtTokenOption =
            this.jwtService.GetUserJwtToken(this.HttpContext.Request.Headers)

        if jwtTokenOption.IsSome then
            let userGuidOption =
                this.jwtService.GetUserGuid(jwtTokenOption.Value)

            if userGuidOption.IsSome then
                let userInfo =
                    userService.GetInfoAboutUserAsync(Guid(userGuidOption.Value.ToString()))
                    |> Async.AwaitTask
                    |> Async.RunSynchronously

                let json = JsonConvert.SerializeObject userInfo

                Result.Ok(json)
            else
                Result.Fail<string>("Error occurred while decoding the jwt token")
        else
            Result.Fail<string>("Error occurred while getting user jwt token")

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

    [<HttpPost>]
    [<Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.PayForSubscription
        (
            [<FromForm>] subscribeId: Guid,
            [<FromForm>] cardNumber: string,
            [<FromForm>] ccid: string,
            [<FromForm>] expirationMonth: int,
            [<FromForm>] expirationYear: int
        ) =
        let jwtTokenOption =
            this.jwtService.GetUserJwtToken(this.HttpContext.Request.Headers)

        if jwtTokenOption.IsSome then
            let userGuidOption =
                this.jwtService.GetUserGuid(jwtTokenOption.Value)

            let cardDto =
                CardDto(
                    CardNumber = cardNumber,
                    CCID = ccid,
                    ExpirationMonth = expirationMonth,
                    ExpirationYear = expirationYear
                )

            if userGuidOption.IsSome then
                paymentService.PaySubscribeAsync(Guid(userGuidOption.Value.ToString()), subscribeId, cardDto)
                |> Async.AwaitTask
                |> Async.RunSynchronously
            else
                Result.Fail<string>("Error occurred while decoding the jwt token")
        else
            Result.Fail<string>("Error occurred while getting user jwt token")
