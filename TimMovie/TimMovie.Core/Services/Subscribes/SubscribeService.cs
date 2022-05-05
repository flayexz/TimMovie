using AutoMapper;
using TimMovie.Core.DTO.Subscribes;
using TimMovie.Core.Entities;
using TimMovie.Core.Query;
using TimMovie.Core.Specifications.InheritedSpecifications.SubscribeSpec;
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
        var query = _userSubscribeRepository.Query
            .Where(new UserSubscribeByUserIdSpec(userId));
        
        var subscribes = new QueryExecutor<UserSubscribe>(query, _userSubscribeRepository)
            .IncludeInResult(subscribe => subscribe.Subscribe)
            .GetEntities();
        
        return MapToUserSubscribeDto(subscribes);
    }

    private IEnumerable<UserSubscribeDto> MapToUserSubscribeDto(IEnumerable<UserSubscribe> userSubscribes)
    {
        return userSubscribes
            .Select(subscribe => _mapper.Map<UserSubscribeDto>(subscribe));
    }
}