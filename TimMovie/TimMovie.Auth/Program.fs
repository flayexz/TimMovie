namespace TimMovie.Auth
#nowarn "20"

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open TimMovie.Auth.AuthStartupSetup
open TimMovie.Core.Classes
open TimMovie.Infrastructure


module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder(args)
        let services = builder.Services  
        let environment = builder.Environment
        let configuration = builder.Configuration
       
        
        
        
        services.AddControllers()
        
        if environment.IsDevelopment() then
            services.AddCors(fun options ->
                options.AddDefaultPolicy(fun builder ->
                        builder.AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin()
                        |> ignore))
        else
            services.AddCors()
        
        services.AddAuthenticationWithJwt()
            .AddAuthorization()
              
        services.AddDbContext(configuration["ConnectionStrings:DefaultConnection"])
        
        services.AddIdentity()
            .AddOpenIdConnect()
            
        let app = builder.Build()
        
        app
            .UseAuthentication()
            .UseAuthorization()
            .UseCors()
            .UseHttpsRedirection()
        
        app.MapControllers()

        app.Run()

        exitCode