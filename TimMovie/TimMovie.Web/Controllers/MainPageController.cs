using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Entities;
using TimMovie.Infrastructure.Database.Repositories;
using TimMovie.Web.ViewModels;

namespace TimMovie.Web.Controllers;

public class MainPageController :Controller
{
    private readonly Repository<Banner> _bannerRepository;
    private readonly Repository<Film> _filmRepository;
    private readonly IMapper _mapper;

    public MainPageController(Repository<Banner> bannerRepository, Repository<Film> filmRepository, IMapper mapper)
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

        return View("MainPage", (banners, films));
        //return Ok();
    }
    
    
}