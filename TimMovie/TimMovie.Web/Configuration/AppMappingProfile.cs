using AutoMapper;
using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Extensions;
using TimMovie.Web.ViewModels;

namespace TimMovie.Web.Configuration;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<RegistrationViewModel, User>()
            .ForMember(x => x.DisplayName,
                opt =>
                    opt.MapFrom(src => src.Email.GetMailName()));
    }
}