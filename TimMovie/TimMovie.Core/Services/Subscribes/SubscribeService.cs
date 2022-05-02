using AutoMapper;
using TimMovie.Core.DTO.Subscribes;
using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services.Subscribes;

public class SubscribeService
{
    private readonly IRepository<UserSubscribe> _userSubscribeRepository;
    private readonly IMapper _mapper;

    public SubscribeService(IRepository<UserSubscribe> userSubscribeRepository, IMapper mapper)
    {
        _userSubscribeRepository = userSubscribeRepository;
        _mapper = mapper;
    }

    public IEnumerable<UserSubscribeDto> GetAllUserSubscribes(Guid userId)
    {
        var userSubscribes = _userSubscribeRepository.Query
            .Where(subscribe => subscribe.SubscribedUser.Id == userId)
            .ToList();
        return MapToUserSubscribeDto(userSubscribes);
    }

    private IEnumerable<UserSubscribeDto> MapToUserSubscribeDto(IEnumerable<UserSubscribe> userSubscribes)
    {
        return userSubscribes
            .Select(subscribe => _mapper.Map<UserSubscribeDto>(subscribe));
    }
}