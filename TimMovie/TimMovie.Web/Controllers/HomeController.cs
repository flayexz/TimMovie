using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Extensions;
using TimMovie.SharedKernel.Interfaces;
using TimMovie.Web.ViewModels;

namespace TimMovie.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    
    public HomeController(ILogger<HomeController> logger, IRepository<User> repository)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    
    public async Task<IActionResult> Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}