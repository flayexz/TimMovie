using AutoMapper;
using TimMovie.Core.DTO.Films;
using TimMovie.Core.DTO.Subscribes;
using TimMovie.Core.Entities;

namespace TimMovie.Core;

public class CoreMappingProfile: Profile
{
    public CoreMappingProfile()
    {
        CreateMap<Film, FilmCardDto>()
            .ForMember(
                dto => dto.FirstGenreName,
                expression => expression.MapFrom(film => film.Genres.First().Name));
        CreateMap<UserSubscribe, UserSubscribeDto>();
        CreateMap<User, FilmForStatusDto>()
            .ForMember(
                film => film.Id,
                expression => expression.MapFrom(user => user.WatchingFilm.Id))
            .ForMember(
                film => film.Title,
                expression => expression.MapFrom(user => user.WatchingFilm.Title));
    }
}