using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.DTO;
using TimMovie.Core.DTO.Films;
using TimMovie.Core.Entities;
using TimMovie.Core.Services.Films;
using TimMovie.SharedKernel.Interfaces;
using TimMovie.Web.ViewModels;
using TimMovie.Web.ViewModels.FilmCard;

namespace TimMovie.Web.Controllers.Navbar;

public class MainPageController :Controller
{
    private readonly IRepository<Banner> _bannerRepository;
    private readonly IRepository<Film> _filmRepository;
    private readonly FilmCardService _filmCardService;
    private readonly IMapper _mapper;

    public MainPageController(IRepository<Banner> bannerRepository, IRepository<Film> filmRepository, IMapper mapper, FilmCardService filmCardService)
    {
        _bannerRepository = bannerRepository;
        _filmRepository = filmRepository;
        _mapper = mapper;
        _filmCardService = filmCardService;
    }

    [HttpGet]
    public async Task<IActionResult> MainPage()
    {
        var films = _mapper.Map<List<FilmMainPageViewModel>>(await _filmRepository.GetAllAsync()).Take(15).ToList();
        var banners = _mapper.Map<List<BannerViewModel>>(await _bannerRepository.GetAllAsync());
        
        
        return View("~/Views/Navbar/MainPage/MainPage.cshtml", (banners, films));
        //return Ok();
    }
    
    public async Task<ActionResult> GetMessage()
    {
        var films = await _filmRepository.GetAllAsync();
        var cards = films.Select(card=> _mapper.Map<FilmMainPageViewModel>(card));
        //return PartialView("~/Views/FilmCard/FilmCard.cshtml", cards);
        return PartialView("~/Views/Navbar/Mainpage/check.cshtml", cards.Take(10));
    }
}