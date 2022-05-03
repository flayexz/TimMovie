namespace TimMovie.WebApi.Controllers.AuthorizationController

open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open OpenIddict.Validation.AspNetCore
open TimMovie.Core.DTO.Account
open TimMovie.Core.Interfaces

[<ApiController>]
[<Route("[controller]/[action]")>]
type AccountController(logger: ILogger<AccountController>, userService: IUserService) =
    inherit ControllerBase()

    [<HttpPost>]
    [<AllowAnonymous>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.Register([<FromForm>] email: string, [<FromForm>] username: string, [<FromForm>] password: string) =
        let userRegistrationDto = UserRegistrationDto()
        userRegistrationDto.Email <- email
        userRegistrationDto.UserName <- username
        userRegistrationDto.Password <- password
        userService.RegisterUserAsync(userRegistrationDto)
        
    [<HttpPost>]
    [<Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.SendConfirmEmail([<FromForm>] email: string, [<FromForm>] urlToAction: string) =
        userService.SendConfirmEmailAsync(email, urlToAction)
        
    [<HttpPost>]
    [<Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.ConfirmEmail([<FromForm>] userId: string, [<FromForm>] code: string) =
        userService.ConfirmEmailAsync(userId, code)
