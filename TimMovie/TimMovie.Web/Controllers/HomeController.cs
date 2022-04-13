using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Interfaces;
using TimMovie.Web.ViewModels;

namespace TimMovie.Web.Controllers;

public class HomeController : Controller
{
    private readonly IUserMessageService userMessageService;
    private readonly IMailService mailService;
    
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger,IUserMessageService userMessageService, IMailService mailService)
    {
        _logger = logger;
        this.userMessageService = userMessageService;
        this.mailService = mailService;
    }

    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Privacy()
    {
        var msg = userMessageService.GenerateMessageTemplate("", "Ваша подписка заканчивается", "", "Уведомление");
        mailService.SendMessageAsync("nin139@mail.ru", msg);
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}