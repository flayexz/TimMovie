using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Classes;
using TimMovie.Core.Entities;
using TimMovie.Core.Enums;
using TimMovie.Core.Services.Films;
using TimMovie.SharedKernel.Interfaces;
using TimMovie.Web.ViewModels.FilmCard;

namespace TimMovie.Web.ViewComponents;

public class CarouselViewComponent : ViewComponent
{
    private readonly FilmCardService _filmCardService;
    private readonly IMapper _mapper;

    public CarouselViewComponent(FilmCardService filmCardService, IMapper mapper)
    {
        _filmCardService = filmCardService;
        _mapper = mapper;
    }

    public IViewComponentResult Invoke(string genreName)
    {
        var cards = _mapper.Map<List<FilmCardViewModel>>(_filmCardService.GetFilmCardsByGenre(genreName, 15));
        return View(cards);
    }
}