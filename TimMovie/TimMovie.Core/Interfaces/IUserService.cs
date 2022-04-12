using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using TimMovie.Core.DTO;
using TimMovie.SharedKernel.Classes;

namespace TimMovie.Core.Interfaces;

public interface IUserService
{
    public Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userRegistrationDto, string? ip);

    public Task<Result> SendConfirmEmailAsync(string userName,string urlToAction);

    public Task<Result> ConfirmEmailAsync(string userId, string code);

   public AuthenticationProperties GetExternalAuthenticationProperties(string provider, string redirectUrl);
    public Task<SignInResult> ExternalLoginCallback();
    public Task<Result> RegisterExternalAsync(ExternalLoginDto externalLoginDto, string? ip);

    public Task<bool> IsEmailExistAsync(string email);
    
    public Task<ExternalLoginInfo?> GetExternalLoginInfoAsync();

}