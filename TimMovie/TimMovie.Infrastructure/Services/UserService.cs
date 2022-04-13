﻿using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using TimMovie.Core.DTO;
using TimMovie.Core.Entities;
using TimMovie.Core.Interfaces;
using TimMovie.Core.Services;
using TimMovie.SharedKernel.Classes;
using TimMovie.SharedKernel.Extensions;

namespace TimMovie.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly SignInManager<User> signInManager;
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;
    private readonly IMailService mailService;
    private readonly IUserMessageService userMessageService;
    private readonly IIpService ipService;
    private readonly CountryService countryService;
    private readonly IVkService vkService;

    public UserService(SignInManager<User> signInManager, UserManager<User> userManager, IMapper mapper,
        IMailService mailService, IUserMessageService userMessageService, IIpService ipService,
        CountryService countryService, IVkService vkService)
    {
        this.signInManager = signInManager;
        this.userManager = userManager;
        this.mapper = mapper;
        this.mailService = mailService;
        this.userMessageService = userMessageService;
        this.ipService = ipService;
        this.countryService = countryService;
        this.vkService = vkService;
    }

    public async Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userRegistrationDto)
    {
        var user = mapper.Map<User>(userRegistrationDto);
        user.RegistrationDate = DateTime.Now;
        user.DisplayName = userRegistrationDto.UserName;
        user.BirthDate = DateOnly.FromDateTime(DateTime.Today);
        if (userRegistrationDto.Ip != null)
            await AddCountryByIpAsync(user, userRegistrationDto.Ip);
        var registerResult = await userManager.CreateAsync(user, userRegistrationDto.Password);
        if (registerResult.Succeeded)
        {
            var userFromDb = await userManager.FindByNameAsync(userRegistrationDto.UserName);
            await UpdateClaimsAsync(userFromDb);
            await userManager.UpdateAsync(userFromDb);
        }

        return registerResult;
    }

    public async Task<Result> SendConfirmEmailAsync(string email, string urlToAction)
    {
        var userFromDb = await userManager.FindByEmailAsync(email);
        if (userFromDb is null)
        {
            return Result.Fail("can`t find user by userName while sending email");
        }

        var token = await userManager.GenerateEmailConfirmationTokenAsync(userFromDb);
        var confirmUrl = CreateUrlToConfirmEmail(urlToAction, userFromDb, token);
        var msg = userMessageService.GenerateConfirmMessage(userFromDb.DisplayName, confirmUrl);
        var result = await mailService.SendMessageAsync(userFromDb.Email,msg);
        return result;
    }

    public async Task<Result> ConfirmEmailAsync(string userId, string code)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
        {
            return Result.Fail("userId or code was empty in confirmEmail");
        }

        var user = await userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return Result.Fail("can`t find user by id in confirmEmail");
        }

        var result = await userManager.ConfirmEmailAsync(user, code);

        if (result.Succeeded)
        {
            await signInManager.SignInAsync(user, false);
            return Result.Ok();
        }

        return Result.Fail("cant confirm user email");
    }

    public AuthenticationProperties GetExternalAuthenticationProperties(string provider, string redirectUrl)
    {
        return signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
    }

    public async Task<SignInResult> ExternalLoginCallback()
    {
        var info = await GetExternalLoginInfoAsync();
        if (info is null)
            return SignInResult.Failed;
        var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, false);
        return result;
    }

    public async Task<Result> RegisterExternalAsync(ExternalLoginDto externalLoginDto)
    {
        if (await IsEmailExistAsync(externalLoginDto.Email))
        {
            return Result.Fail($"почта {externalLoginDto.Email} уже занята");
        }

        var info = await GetExternalLoginInfoAsync();
        if (info is null)
        {
            return Result.Fail("не удалось получить регистрационную информацию из внешнего поставщика");
        }

        var id = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var vkInfoResult = await vkService.GetUserInfoByIdAsync(id);
        if (vkInfoResult.Succeeded)
        {
            var user = await CreateUserByVkRegistrationAsync(externalLoginDto.Email, vkInfoResult.Value,
                externalLoginDto.Ip);
            var createUserResult = await userManager.CreateAsync(user);
            if (!createUserResult.Succeeded)
            {
                return Result.Fail("ошибка при создании пользователя");
            }

            var addLoginResult = await userManager.AddLoginAsync(user, info);
            if (!addLoginResult.Succeeded)
            {
                return Result.Fail(addLoginResult.Errors.First().Description);
            }

            await UpdateClaimsAsync(user);
            await signInManager.UpdateExternalAuthenticationTokensAsync(info);
            return Result.Ok();
        }

        return Result.Fail(vkInfoResult.Error);
    }

    public async Task<bool> IsEmailExistAsync(string email)
    {
        var userMail = await userManager.FindByEmailAsync(email);
        return userMail is not null;
    }

    public async Task<ExternalLoginInfo?> GetExternalLoginInfoAsync() =>
        await signInManager.GetExternalLoginInfoAsync();


    private async Task UpdateClaimsAsync(User user)
    {
        await userManager.AddClaimAsync(user, new Claim(ClaimTypes.DateOfBirth, user.BirthDate.ToString()));
    }

    private async Task AddCountryByIpAsync(User user, string ip)
    {
        if (string.IsNullOrEmpty(ip)) return;
        var userCountryResult = await ipService.GetCountryByIpAsync(ip);
        if (userCountryResult.Succeeded)
        {
            var countryFromDb = countryService.FindByName(userCountryResult.Value);
            if (countryFromDb != null)
            {
                user.Country = countryFromDb;
            }
        }
    }

    private async Task<User> CreateUserByVkRegistrationAsync(string email, Core.Classes.VkUserInfo vkInfo, string? ip)
    {
        var user = new User
        {
            UserName = email.GetMailName().AddRandomEnd(), Email = email,
            DisplayName = vkInfo.FirstName + " " + vkInfo.LastName,
            BirthDate = vkInfo.Birthday,
            RegistrationDate = DateTime.Now
        };
        if (ip != null)
            await AddCountryByIpAsync(user, ip);
        return user;
    }

    private string CreateUrlToConfirmEmail(string urlToAction, User user, string token)
    {
        var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
        queryString.Add("userId", user.Id.ToString());
        queryString.Add("code", token);
        return urlToAction + "?" + queryString;
    }
}