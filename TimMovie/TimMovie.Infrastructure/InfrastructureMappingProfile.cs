using AutoMapper;
using TimMovie.Core.DTO.Users;
using TimMovie.Core.Entities;

namespace TimMovie.Infrastructure;

public class InfrastructureMappingProfile: Profile
{
    public InfrastructureMappingProfile()
    {
        CreateMap<User, UserInfoDto>()
            .ForMember(
                x => x.UserStatusEnum,
                e => e.MapFrom(src => src.Status.UserStatusEnum));
        CreateMap<User, UserInfoForChatDto>();
    }
}