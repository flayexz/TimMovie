using AutoMapper;
using TimMovie.Core.DTO.Films;
using TimMovie.Core.DTO.Genre;
using TimMovie.Core.DTO.Messages;
using TimMovie.Core.DTO.Notifications;
using TimMovie.Core.DTO.Person;
using TimMovie.Core.DTO.Subscribes;
using TimMovie.Core.DTO.WatchedFilms;
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
        CreateMap<Subscribe, SubscribeDto>()
            .ForMember(
                subscribeDto => subscribeDto.Films,
                expression => expression.MapFrom(subscribe => subscribe.Films)
                )
            .ForMember(
                subscribeDto => subscribeDto.Genres,
                expression => expression.MapFrom(subscribe => subscribe.Genres)
            ).ReverseMap();
        CreateMap<Film, SubscribeFilmDto>();
        CreateMap<Genre, GenreDto>().ReverseMap();
        CreateMap<User, FilmForStatusDto>()
            .ForMember(
                film => film.Id,
                expression => expression.MapFrom(user => user.WatchingFilm.Id))
            .ForMember(
                film => film.Title,
                expression => expression.MapFrom(user => user.WatchingFilm.Title));
        CreateMap<Producer, PersonDto>();
        CreateMap<Actor, PersonDto>();
        CreateMap<UserFilmWatched, WatchedFilmDto>()
            .ForMember(
                x => x.WatchedDate,
                e => e.MapFrom(src => DateOnly.FromDateTime(src.Date)))
            .ForMember(x => x.Title,
                e => e.MapFrom(src => src.Film.Title))
            .ForMember(x => x.Image,
                e => e.MapFrom(src => src.Film.Image));
        CreateMap<Message, MessageDto>();
        CreateMap<Notification, NotificationDto>();
        CreateMap<NewMessageDto, Message>();
        CreateMap<Film, PersonFilmDto>();
    }
}