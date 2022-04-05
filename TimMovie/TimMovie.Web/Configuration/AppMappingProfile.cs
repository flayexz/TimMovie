using AutoMapper;
using TimMovie.Core.Entities;
using TimMovie.Web.ViewModels;

namespace TimMovie.Web.Configuration;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<RegistrationViewModel, User>();
    }
}