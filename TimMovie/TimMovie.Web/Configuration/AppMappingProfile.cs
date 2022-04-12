using AutoMapper;
using TimMovie.Core.DTO;
using TimMovie.Core.Entities;
using TimMovie.Web.ViewModels;

namespace TimMovie.Web.Configuration;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<RegistrationViewModel, UserRegistrationDto>();
        CreateMap<UserRegistrationDto, User>();
        CreateMap<FilmMainPageViewModel, Film>().ReverseMap();
        CreateMap<BannerViewModel, Banner>().ReverseMap();
        CreateMap<ExternalLoginViewModel, ExternalLoginDto>();
    }
}