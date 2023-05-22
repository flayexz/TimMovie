using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.DTO.Account;
using TimMovie.Core.Entities;
using TimMovie.Core.Interfaces;
using TimMovie.Core.Services.Banners;
using TimMovie.Web.ViewModels;
using TimMovie.Web.ViewModels.Account;

namespace TimMovie.Web.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly ILogger<AccountController> logger;
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;
    private readonly IUserService userService;
    private readonly BannerService bannerService;

    public AccountController(UserManager<User> userManager,
        IMapper mapper,
        ILogger<AccountController> logger,
        IUserService userService)
    {
        this.userManager = userManager;
        this.mapper = mapper;
        this.logger = logger;
        this.userService = userService;
    }

    [HttpGet]
    public IActionResult Registration(string? returnUrl)
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

        var userDto = mapper.Map<UserRegistrationDto>(model);
        var registerUserResult = await userService.RegisterUserAsync(userDto);

        if (!registerUserResult.Succeeded)
        {
            AddErrors(registerUserResult);
            return View(model);
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> LoginAsync(string login, string password, bool rememberMe)
    {
        var loginDto = new LoginDto
        {
            Login = login,
            Password = password,
            RememberMe = rememberMe
        };


        var loginResult = await userService.LoginAsync(loginDto);

        if (loginResult.Succeeded)
        {
            logger.LogInformation($"пользователь {login} вошел в свой акканут");
            return Ok();
        }

        if (loginResult.IsNotAllowed)
        {
            var userFromDb = await userManager.FindByNameAsync(login) ?? await userManager.FindByEmailAsync(login);
            return PartialView("MailSend", (userFromDb.Email, userFromDb.DisplayName));
        }

        logger.LogInformation($"неудачная попытка входа с использованием логина {login}");
        return Ok("Неверный(е) логин/почта или пароль");
    }

    [HttpGet]
    public IActionResult Denied()
    {
        return View("Denied");
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }
}