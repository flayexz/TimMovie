using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Interfaces;
using TimMovie.Web.ViewModels;

namespace TimMovie.Web.Controllers.Navbar;

public class MainPageController :Controller
{
    private readonly IRepository<Banner> _bannerRepository;
    private readonly IRepository<Film> _filmRepository;
    private readonly IMapper _mapper;

    public MainPageController(IRepository<Banner> bannerRepository, IRepository<Film> filmRepository, IMapper mapper)
    {
        _bannerRepository = bannerRepository;
        _filmRepository = filmRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> MainPage()
    {
        var films = _mapper.Map<List<FilmMainPageViewModel>>(await _filmRepository.GetAllAsync());
        var banners = _mapper.Map<List<BannerViewModel>>(await _bannerRepository.GetAllAsync());

        return View("~/Views/Navbar/MainPage/MainPage.cshtml", (banners, films));
        //return Ok();
    }
}