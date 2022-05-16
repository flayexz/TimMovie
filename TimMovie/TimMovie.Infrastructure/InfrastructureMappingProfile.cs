using AutoMapper;
using TimMovie.Core.DTO.Users;
using TimMovie.Core.Entities;

namespace TimMovie.Infrastructure;

public class InfrastructureMappingProfile: Profile
{
    public InfrastructureMappingProfile()
    {
        CreateMap<User, UserInfoDto>();
        CreateMap<User, UserInfoForChatDto>();
    }
}