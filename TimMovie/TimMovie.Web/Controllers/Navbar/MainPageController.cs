using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.DTO;
using TimMovie.Core.DTO.Films;
using TimMovie.Core.Enums;
using TimMovie.Core.Services;
using TimMovie.Core.Services.Banners;
using TimMovie.Core.Services.Films;
using TimMovie.Web.ViewModels;
using TimMovie.Web.ViewModels.FilmCard;

namespace TimMovie.Web.Controllers.Navbar;

public class MainPageController : Controller
{
    private readonly FilmCardService _filmCardService;
    private readonly BannerService _bannerService;
    private readonly IMapper _mapper;

    public MainPageController(IMapper mapper, FilmCardService filmCardService, BannerService bannerService)
    {
        _mapper = mapper;
        _filmCardService = filmCardService;
        _bannerService = bannerService;
    }


    [HttpGet]
    public IActionResult MainPage()
    {
        var banners = _mapper.Map<List<BannerViewModel>>(_bannerService.GetBanners());
        return View("~/Views/Navbar/MainPage/MainPage.cshtml", (banners));
    }
}