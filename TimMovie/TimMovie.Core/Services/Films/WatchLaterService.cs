using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimMovie.Core.DTO.Films;
using TimMovie.Core.Entities;
using TimMovie.Core.Query;
using TimMovie.Core.Query.Films;
using TimMovie.Core.Services.WatchedFilms;
using TimMovie.Core.Specifications.InheritedSpecifications;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services.Films;

public class WatchLaterService
{
    private readonly FilmService _filmService;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public WatchLaterService(
        IMapper mapper,
        UserManager<User> userManager, FilmService filmService)
    {
        _mapper = mapper;
        _userManager = userManager;
        _filmService = filmService;
    }


    public List<BigFilmCardDto> GetWatchLaterFilmsAsync(Guid userId, int take, int skip)
    {
        var filmsWatchLater = _userManager.Users
            .Where(u => u.Id == userId)
            .Select(u => u.FilmsWatchLater.Skip(skip).Take(take))
            .FirstOrDefault();

        if (filmsWatchLater is null) return new List<BigFilmCardDto>();

        var filmDtoList = filmsWatchLater.Select(f => _filmService.GetDbFilmById(f.Id));
        return filmDtoList
            .Select(filmDto => _mapper.Map<BigFilmCardDto>(filmDto))
            .ToList();
    }
}