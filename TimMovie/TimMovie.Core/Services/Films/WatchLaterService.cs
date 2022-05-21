using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TimMovie.Core.DTO.Films;
using TimMovie.Core.Entities;
using TimMovie.Core.Services.WatchedFilms;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services.Films;

public class WatchLaterService
{
    private readonly IRepository<Film> _filmRepository;
    private readonly IRepository<User> _userRepository;
    private readonly Lazy<WatchedFilmService> _watchedFilmService;
    private readonly FilmService _filmService;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public WatchLaterService(
        IRepository<Film> filmRepository,
        IRepository<User> userRepository,
        IMapper mapper,
        Lazy<WatchedFilmService> watchedFilmService, UserManager<User> userManager, FilmService filmService)
    {
        _filmRepository = filmRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _watchedFilmService = watchedFilmService;
        _userManager = userManager;
        _filmService = filmService;
    }

    public async Task<List<BigFilmCardDto>> GetWatchLaterFilmsAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        var filmsWatchLater = user.FilmsWatchLater;
        return filmsWatchLater
            .Select(film =>
            {
                var filmCard = _mapper.Map<BigFilmCardDto>(film);
                filmCard.Rating = _filmService.GetRating(film);
                return filmCard;
            })
            .ToList();
    }
}