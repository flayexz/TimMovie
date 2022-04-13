using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using TimMovie.Core.DTO;
using TimMovie.SharedKernel.Classes;

namespace TimMovie.Core.Interfaces;

public interface IUserService
{
    public Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userRegistrationDto);

    public Task<Result> SendConfirmEmailAsync(string email,string urlToAction);

    public Task<Result> ConfirmEmailAsync(string userId, string code);

   public AuthenticationProperties GetExternalAuthenticationProperties(string provider, string redirectUrl);
    public Task<SignInResult> ExternalLoginCallback();
    public Task<Result> RegisterExternalAsync(ExternalLoginDto externalLoginDto);

    public Task<bool> IsEmailExistAsync(string email);
    
    public Task<ExternalLoginInfo?> GetExternalLoginInfoAsync();

}