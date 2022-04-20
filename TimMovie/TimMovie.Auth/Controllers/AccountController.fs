namespace TimMovie.Auth.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

[<ApiController>]
[<Route("api/connect")>]
type AccountController (logger : ILogger<AccountController>) =
    inherit ControllerBase()
    
    [<HttpPost>]
    member this.Post() =
        let request = Microsoft.AspNetCore.HttpContext.GetOpenIddictServerRequest()
        if(request.IsPassword)
        