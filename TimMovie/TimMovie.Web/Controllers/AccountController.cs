using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Classes;
using TimMovie.Core.Entities;
using TimMovie.Core.Interfaces;
using TimMovie.Infrastructure.Database.Repositories;
using TimMovie.SharedKernel.Classes;
using TimMovie.SharedKernel.Extensions;
using TimMovie.Web.ViewModels;

namespace TimMovie.Web.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly IVkService vkService;
    private readonly ILogger<AccountController> logger;
    private readonly IIpService ipService;
    private readonly CountryRepository countryRepository;
    private readonly UserManager<User> userManager;
    private readonly IUserMessageService userMessageService;
    private readonly IMailService mailService;
    private readonly SignInManager<User> signInManager;
    private readonly IMapper mapper;

    public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, IMapper mapper,
        IUserMessageService userMessageService, IMailService mailService, ILogger<AccountController> logger,
        IIpService ipService, CountryRepository countryRepository, IVkService vkService)
    {
        this.signInManager = signInManager;
        this.userManager = userManager;
        this.mapper = mapper;
        this.userMessageService = userMessageService;
        this.mailService = mailService;
        this.logger = logger;
        this.ipService = ipService;
        this.countryRepository = countryRepository;
        this.vkService = vkService;
    }

    [HttpGet]
    public IActionResult Registration()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegistrationAsync(RegistrationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await CreateUserAsync(model);
        var result = await userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            AddErrors(result);
            return View(model);
        }

        await userManager.UpdateAsync(user);
        await AddClaimsAsync(user);
        var sendResult = await SendConfirmEmailAsync(user);
        if (sendResult.Succeeded)
        {
            return PartialView("MailSend", (user.Email, user.DisplayName));
        }

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmailAsync(string userId, string code)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
        {
            return View("Error");
        }

        var user = await userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return View("Error");
        }

        var result = await userManager.ConfirmEmailAsync(user, code);

        if (result.Succeeded)
        {
            await signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }

        return View("Error");
    }

    [HttpPost]
    public IActionResult RegisterByVk(string provider, string returnUrl)
    {
        var redirectUrl = Url.Action("ExternalLoginCallback", new { returnUrl });
        var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }

    [AllowAnonymous]
    public async Task<IActionResult> ExternalLoginCallback(string returnUrl)
    {
        var info = await signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            return RedirectToAction("Registration");
        }

        var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, false);

        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }

        if (result.IsNotAllowed)
        {
            var userFromDb = await userManager.FindByEmailAsync(info.Principal.FindFirstValue(ClaimTypes.Email));
            return PartialView("MailSend", (userFromDb.Email, userFromDb.DisplayName));
        }

        return RedirectToAction("RegisterExternal",
            new { ReturnUrl = returnUrl, Email = info.Principal.FindFirstValue(ClaimTypes.Email) });
    }

    [AllowAnonymous]
    public IActionResult RegisterExternal(ExternalLoginViewModel model)
    {
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> RegisterExternalAsync(ExternalLoginViewModel model)
    {
        var userMail = await userManager.FindByEmailAsync(model.Email);
        if (userMail != null)
        {
            ModelState.AddModelError(string.Empty, $"почта {model.Email} уже занята");
            return View(model);
        }

        var info = await signInManager.GetExternalLoginInfoAsync();
        var id = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var registerResult = await RegisterUserByVkAsync(id, model.Email, info);

        if (registerResult.Succeeded)
        {
            var userFromDb = await userManager.FindByEmailAsync(model.Email);
            return PartialView("MailSend", (userFromDb.Email, userFromDb.DisplayName));
        }

        return View("Registration");
    }


    private async Task<Result> RegisterUserByVkAsync(string id, string email, ExternalLoginInfo info)
    {
        var vkInfoResult = await vkService.GetUserInfoByIdAsync(id);
        if (vkInfoResult.Succeeded)
        {
            var user = await CreateUserAsync(email, vkInfoResult.Value);
            var createUserResult = await RegisterUserByExternalProviderAsync(info, user);
            if (createUserResult.Succeeded)
            {
                return Result.Ok();
            }
        }

        return Result.Fail(vkInfoResult.Error);
    }

    private async Task<Result> RegisterUserByExternalProviderAsync(ExternalLoginInfo info, User user)
    {
        var result = await userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            AddErrors(result);
            return Result.Fail(result.Errors.First().Description);
        }

        result = await userManager.AddLoginAsync(user, info);
        if (result.Succeeded)
        {
            await AddClaimsAsync(user);
            await signInManager.UpdateExternalAuthenticationTokensAsync(info);
            await SendConfirmEmailAsync(user);
            return Result.Ok();
        }

        AddErrors(result);
        return Result.Fail(result.Errors.First().Description);
    }

    private async Task<User> CreateUserAsync(string email, VkUserInfo vkInfo)
    {
        var user = new User
        {
            UserName = email.GetMailName().AddRandomEnd(), Email = email,
            DisplayName = vkInfo.FirstName + " " + vkInfo.LastName,
            BirthDate = vkInfo.Birthday,
            RegistrationDate = DateTime.Now
        };
        await AddCountryByIpAsync(user);
        return user;
    }

    [HttpPost]
    [ActionName("SendConfirmEmail")]
    public async Task<IActionResult> SendConfirmEmailAction(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return BadRequest();
        }

        await SendConfirmEmailAsync(user);
        return PartialView("MailSend", (user.Email, user.DisplayName));
    }

    private async Task<Result> SendConfirmEmailAsync(User user)
    {
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmUrl = Url.Action(
            "ConfirmEmail",
            "Account",
            new { userId = user.Id, code = token },
            HttpContext.Request.Scheme);
        var msg = userMessageService.GenerateConfirmMessage(user.DisplayName, user.Email, confirmUrl!);
        var result = await mailService.SendMessageAsync(msg);
        if (result.IsFailure)
        {
            logger.Log(LogLevel.Error, result.Error);
            return Result.Fail(result.Error);
        }

        logger.Log(LogLevel.Information, $"send email msg to {user.Email}");
        return Result.Ok();
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }

    private async Task<User> CreateUserAsync(RegistrationViewModel model)
    {
        var user = mapper.Map<User>(model);
        user.RegistrationDate = DateTime.Now;
        user.DisplayName = model.UserName;
        user.BirthDate = DateOnly.FromDateTime(DateTime.Today);
        await AddCountryByIpAsync(user);
        return user;
    }

    private async Task AddClaimsAsync(User user)
    {
        await userManager.AddClaimAsync(user, new Claim(ClaimTypes.DateOfBirth, user.BirthDate.ToString()));
    }

    private async Task AddCountryByIpAsync(User user)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userCountryResult = await ipService.GetCountryByIpAsync(ip);
        if (userCountryResult.Succeeded)
        {
            var countryFromDb = await countryRepository.FindByNameAsync(userCountryResult.Value);
            if (countryFromDb != null)
            {
                user.Country = countryFromDb;
            }
        }
    }
}