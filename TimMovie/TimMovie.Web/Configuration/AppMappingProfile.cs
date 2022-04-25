using AutoMapper;
using TimMovie.Core.DTO;
using TimMovie.Core.DTO.Account;
using TimMovie.Core.DTO.Films;
using TimMovie.Core.DTO.Payment;
using TimMovie.Core.Entities;
using TimMovie.Web.ViewModels;
using TimMovie.Web.ViewModels.Account;
using TimMovie.Web.ViewModels.FilmCard;
using TimMovie.Web.ViewModels.Payment;

namespace TimMovie.Web.Configuration;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<RegistrationViewModel, UserRegistrationDto>();
        CreateMap<UserRegistrationDto, User>();
        CreateMap<Banner, BannerViewModel>();
        CreateMap<ExternalLoginViewModel, ExternalLoginDto>();
        CreateMap<FilmCardDto, FilmCardViewModel>();
        CreateMap<Film, FilmCardViewModel>();
        CreateMap<LoginViewModel, LoginDto>().ReverseMap();
        CreateMap<SubscribePaymentViewModel, SubscribePaymentDto>().ReverseMap();
        CreateMap<CardViewModel, CardDto>().ReverseMap();
    }
}