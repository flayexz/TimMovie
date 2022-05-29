namespace TimMovie.WebApi.Services.JwtService

open System.Collections.Generic
open System.IdentityModel.Tokens.Jwt
open System.Linq
open System.Net
open System.Net.Http

type JwtService() =
    member this.GetUserJwtToken(headers: Microsoft.AspNetCore.Http.IHeaderDictionary) =
        if (headers.ContainsKey("Authorization")
            && headers["Authorization"].Count <> 0) then
            Some(headers["Authorization"])
        else
            None

    member this.GetUserGuid(headerRecord: string) =
        if (headerRecord = null) then
            None
        else
            let parts = headerRecord.Split [| ' ' |]

            if (parts.Length <> 2) then
                None
            else
                let token = parts[1]

                if (token = null) then
                    None
                else
                    let handler = JwtSecurityTokenHandler()
                    let jwtSecurityToken = handler.ReadJwtToken(token)

                    if (jwtSecurityToken = null) then
                        None
                    else
                        Some(
                            Enumerable
                                .FirstOrDefault(
                                    jwtSecurityToken.Claims,
                                    (fun claim -> claim.Type = "sub")
                                )
                                .Value
                        )
    
    member this.Authorize(route: string, username: string, password: string) =
        let data = List<KeyValuePair<string, string>>()
        data.Add(KeyValuePair<string, string>(nameof(username), username))
        data.Add(KeyValuePair<string, string>(nameof(password), password))
        data.Add(KeyValuePair<string, string>("grant_type", "password"))
        let client = new HttpClient()
        let response = client.PostAsync(route, new FormUrlEncodedContent(data))
                       |> Async.AwaitTask
                       |> Async.RunSynchronously
        if response.StatusCode <> HttpStatusCode.OK then
            TimMovie.SharedKernel.Classes.Result.Fail<string>($"Response status code is {response.StatusCode}")
        else
            let content = response.Content.ReadAsStringAsync()
                          |> Async.AwaitTask
                          |> Async.RunSynchronously
            TimMovie.SharedKernel.Classes.Result.Ok(content)
