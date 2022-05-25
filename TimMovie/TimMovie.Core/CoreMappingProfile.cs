using AutoMapper;
using TimMovie.Core.DTO.Films;
using TimMovie.Core.DTO.Messages;
using TimMovie.Core.DTO.Person;
using TimMovie.Core.DTO.Subscribes;
using TimMovie.Core.DTO.WatchedFilms;
using TimMovie.Core.Entities;

namespace TimMovie.Core;

public class CoreMappingProfile : Profile
{
    public CoreMappingProfile()
    {
        CreateMap<Film, FilmCardDto>()
            .ForMember(
                dto => dto.FirstGenreName,
                expression => expression.MapFrom(film => film.Genres.First().Name));
        CreateMap<UserSubscribe, UserSubscribeDto>();
        CreateMap<Subscribe, SubscribeDto>();
        CreateMap<Film, FilmDto>();
        CreateMap<User, FilmForStatusDto>()
            .ForMember(
                film => film.Id,
                expression => expression.MapFrom(user => user.WatchingFilm.Id))
            .ForMember(
                film => film.Title,
                expression => expression.MapFrom(user => user.WatchingFilm.Title));
        CreateMap<Producer, FilmProducerDto>();
        CreateMap<Actor, FilmActorDto>();
        CreateMap<UserFilmWatched, WatchedFilmDto>()
            .ForMember(
                x => x.WatchedDate,
                e => e.MapFrom(src => DateOnly.FromDateTime(src.Date)))
            .ForMember(x => x.Title,
                e => e.MapFrom(src => src.Film.Title))
            .ForMember(x => x.Image,
                e => e.MapFrom(src => src.Film.Image));
        CreateMap<Message, MessageDto>();
        CreateMap<NewMessageDto, Message>();
        CreateMap<FilmDto, BigFilmCardDto>()
            .ForMember(f => f.Producer,
                e =>
                    e.MapFrom(src => src.Producers.First()))
            .ForMember(f => f.CountryName, e => e.MapFrom(src => src.Country.Name))
            .ForMember(f => f.Image, e => e.MapFrom(src => src.Image));
    }
}