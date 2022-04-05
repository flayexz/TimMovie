using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Entities;
using TimMovie.Core.Interfaces;
using TimMovie.Web.ViewModels;

namespace TimMovie.Web.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly ILogger<AccountController> logger; 
    private readonly UserManager<User> userManager;
    private readonly IUserMessageService userMessageService;
    private readonly IMailService mailService;
    private readonly SignInManager<User> signInManager;
    private readonly IMapper mapper;

    public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, IMapper mapper,
        IUserMessageService userMessageService, IMailService mailService, ILogger<AccountController> logger)
    {
        this.signInManager = signInManager;
        this.userManager = userManager;
        this.mapper = mapper;
        this.userMessageService = userMessageService;
        this.mailService = mailService;
        this.logger = logger;
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

        var user = mapper.Map<User>(model);
        user.RegistrationDate = DateTime.Now;
        var result = await userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            AddErrors(result);
            return View(model);
        }

        await userManager.UpdateAsync(user);
        await SendConfirmEmailAsync(user);
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
            return RedirectToAction("Index", "Home");
        }

        return View("Error");
    }

    private async Task SendConfirmEmailAsync(User user)
    {
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

        var confirmUrl = Url.Action(
            "ConfirmEmail",
            "Account",
            new { userId = user.Id, code = token },
            HttpContext.Request.Scheme);

        var msg = userMessageService.GenerateConfirmMessage(user.UserName, user.Email, confirmUrl!);
        var result = await mailService.SendMessageAsync(msg);
        if(result.IsFailure)
            logger.Log(LogLevel.Error,result.Error);
        else
        {
            logger.Log(LogLevel.Information,$"send email msg to {user.Email}");
        }
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("CustomError", error.Description);
        }
    }
}