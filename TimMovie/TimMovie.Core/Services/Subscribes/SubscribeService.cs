using AutoMapper;
using TimMovie.Core.DTO.Subscribes;
using TimMovie.Core.Entities;
using TimMovie.Core.Query;
using TimMovie.Core.Specifications.InheritedSpecifications.SubscribeSpec;
using TimMovie.Core.Specifications.StaticSpecification;
using TimMovie.SharedKernel.Classes;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services.Subscribes;

public class SubscribeService
{
    private readonly IRepository<UserSubscribe> _userSubscribeRepository;
    private readonly IMapper _mapper;
    private const int SubscribeMonthsDuration = 1;

    public SubscribeService(IRepository<UserSubscribe> userSubscribeRepository, IMapper mapper)
    {
        _userSubscribeRepository = userSubscribeRepository;
        _mapper = mapper;
    }

    public IEnumerable<UserSubscribeDto> GetAllActiveUserSubscribes(Guid userId)
    {
        var query = _userSubscribeRepository.Query
            .Where(new UserSubscribeByUserIdSpec(userId) && SubscribeSpec.ActiveSubscribe);
        
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
    public async Task AddUserToSubscribeAsync(User user, Subscribe subscribe)
    {
        await AddUserToSubscribeAsync(user, subscribe, DateTime.Now, DateTime.Now.AddMonths(SubscribeMonthsDuration));
    }
    
    
    public async Task AddUserToSubscribeAsync(User user, Subscribe subscribe,DateTime startDate ,DateTime endDate)
    {
        var query = _userSubscribeRepository.Query
            .Where(new UserSubscribeByUserIdSpec(user.Id) && new UserSubscribesBySubscribeIdSpec(subscribe.Id) &&
                   SubscribeSpec.ActiveSubscribe);
        
        if (query.Any())
        {
            await ExtendUserSubscribeAsync(query.First());
            return;
        }
        
        var userSubscribe = new UserSubscribe
        {
            EndDate = endDate,
            StartDay = startDate,
            SubscribedUser = user,
            Subscribe = subscribe
        };
        await _userSubscribeRepository.AddAsync(userSubscribe);
        await _userSubscribeRepository.SaveChangesAsync();
    }

    private async Task ExtendUserSubscribeAsync(UserSubscribe userSubscribe)
    {
        userSubscribe.EndDate = userSubscribe.EndDate.AddMonths(SubscribeMonthsDuration);
        await _userSubscribeRepository.SaveChangesAsync();
    }
    
}