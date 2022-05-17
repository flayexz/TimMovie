namespace TimMovie.WebApi.Controllers.AuthorizationController

open System
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open OpenIddict.Validation.AspNetCore
open TimMovie.Core.Interfaces

[<ApiController>]
[<Route("[controller]/[action]")>]
type MainPageController(logger: ILogger<MainPageController>, searchEntityService: ISearchEntityService,
                        subscribeService : ISubscribeService) as this =
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
    member _.GetAllSubscribes([<FromForm>] namePart: string, [<FromForm>] take: int , [<FromForm>] skip: int) =
          if (namePart = null) then
            subscribeService.GetSubscribesByNamePart(namePart)
          else
            subscribeService.GetSubscribesByNamePart(namePart, take, skip);
          
