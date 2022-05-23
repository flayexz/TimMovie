namespace TimMovie.WebApi.Controllers.AuthorizationController

open System
open System.IdentityModel.Tokens.Jwt
open System.Text.Json
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open OpenIddict.Validation.AspNetCore
open TimMovie.Core.Interfaces
open TimMovie.Core.Services.Subscribes

[<ApiController>]
[<Route("[controller]/[action]")>]
type MainPageController
    (
        logger: ILogger<MainPageController>,
        searchEntityService: ISearchEntityService,
        subscribeService: ISubscribeService
    ) as this =
    inherit ControllerBase()

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
    member _.GetAllSubscribes([<FromForm>] namePart: string) =
        let headerToken =
            this.HttpContext.Request.Headers
            
        let subscribes =
            subscribeService.GetSubscribesByNamePart(namePart)

        JsonSerializer.Serialize subscribes

    [<HttpPost>]
    [<Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.GetAllSubscribesWithPagination
        (
            [<FromForm>] namePart: string,
            [<FromForm>] take: int,
            [<FromForm>] skip: int
        ) =
        let subscribes =
            subscribeService.GetSubscribesByNamePart(namePart, take, skip)

        JsonSerializer.Serialize subscribes
        
//    member private _.GetUserIdFromJwtToken(token : string) = 
//        let handler = JwtSecurityTokenHandler()
//        let jwtSecurityToken = handler.ReadJwtToken(token)
//        let headerToken =
//            this.HttpContext.Request.Headers;
//        "123"
//        let jwtSecurityToken = handler.ReadJwtToken(token = token1)
//        "123"
//        var token = "[encoded jwt]";  
//var handler = new JwtSecurityTokenHandler();
//var jwtSecurityToken = handler.ReadJwtToken(token);
