using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace TimMovie.Web.Configuration;

public static class ConfigureVkontakte
{
    public static AuthenticationBuilder AddVkontakte(this AuthenticationBuilder builder,IConfiguration configuration)
    {
        builder.AddOAuth("VK", "VKontakte", config =>
        {
            config.ClientId =  configuration["VkSettings:AppId"];
            config.ClientSecret =  configuration["VkSettings:AppSecret"];
            config.ClaimsIssuer = "VKontakte";
            config.CallbackPath = new PathString("/signin-vkontakte-token");
            config.AuthorizationEndpoint = "https://oauth.vk.com/authorize";
            config.TokenEndpoint = "https://oauth.vk.com/access_token";
            config.Scope.Add("email");
            config.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "user_id");
            config.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            config.SaveTokens = true;
            config.Events = new OAuthEvents
            {
                OnCreatingTicket = context =>
                {
                    context.RunClaimActions(context.TokenResponse.Response.RootElement);
                    return Task.CompletedTask;
                }
            };
        });

        return builder;
    }
}