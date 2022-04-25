using AutoMapper;
using TimMovie.Core.DTO.Films;
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
    }
}