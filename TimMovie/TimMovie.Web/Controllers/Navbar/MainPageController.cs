using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Services.Banners;
using TimMovie.Core.Services.Films;
using TimMovie.SharedKernel.Classes;
using TimMovie.Web.Extensions;
using TimMovie.Web.ViewComponents;
using TimMovie.Web.ViewModels;
using TimMovie.Web.ViewModels.FilmCard;

namespace TimMovie.Web.Controllers.Navbar;

public class MainPageController : Controller
{
    private readonly BannerService _bannerService;
    private readonly IMapper _mapper;
    private readonly FilmCardService filmCardService;

    public MainPageController(IMapper mapper, FilmCardService filmCardService, BannerService bannerService)
    {
        _mapper = mapper;
        _bannerService = bannerService;
        this.filmCardService = filmCardService;
    }


    [HttpGet]
    public IActionResult MainPage()
    {
        var banners = _mapper.Map<List<BannerViewModel>>(_bannerService.GetBanners());
        return View("~/Views/Navbar/MainPage/MainPage.cshtml", banners);
    }

    [HttpGet]
    public IActionResult GetRecommendationResult()
    {
        var userId = User.GetUserId();
        if (!User.Identity.IsAuthenticated || userId is null)
        {
            return Ok();
        }

        var recommendationsResult = filmCardService.GetFilmRecommendationsByUserId(userId.Value, 8);

        if (recommendationsResult.IsFailure)
        {
            return Ok();
        }


        return PartialView("Components/Carousel/CarouselWithoutGenre",
            _mapper.Map<List<FilmCardViewModel>>(recommendationsResult.Value));
    }
}