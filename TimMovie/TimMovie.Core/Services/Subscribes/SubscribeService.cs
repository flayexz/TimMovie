using Autofac;
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
    private readonly IRepository<Subscribe> _subscribesRepository;
    private readonly IMapper _mapper;
    private const int SubscribeMonthsDuration = 1;

    public SubscribeService(IRepository<UserSubscribe> userSubscribeRepository, IRepository<Subscribe> subscribesRepository, IMapper mapper)
    {
        _userSubscribeRepository = userSubscribeRepository;
        _subscribesRepository = subscribesRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// при namePart == null будут возвращены все подписки
    /// </summary>
    /// <param name="namePart"></param>
    /// <param name="take"></param>
    /// <param name="skip"></param>
    /// <returns></returns>
    public IEnumerable<SubscribeDto> GetSubscribesByNamePart(string? namePart, int take, int skip)
    {
        var query = _subscribesRepository.Query
            .Where(new UserSubscribeByNamePart(namePart))
            .Skip(skip)
            .Take(take);
        var subscribes = new QueryExecutor<Subscribe>(query, _subscribesRepository)
            .IncludeInResult(subscribe => subscribe.Films)
            .IncludeInResult(subscribe => subscribe.Genres)
            .GetEntities();
        return MapToSubscribeDto(subscribes);
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

    private IEnumerable<SubscribeDto> MapToSubscribeDto(IEnumerable<Subscribe> userSubscribes) =>
        userSubscribes.Select(s => _mapper.Map<SubscribeDto>(s));

    private IEnumerable<UserSubscribeDto> MapToUserSubscribeDto(IEnumerable<UserSubscribe> userSubscribes)
    {
        return userSubscribes
            .Select(subscribe => _mapper.Map<UserSubscribeDto>(subscribe));
    }

    public async Task AddUserToSubscribeAsync(User user, Subscribe subscribe)
    {
        await AddUserToSubscribeAsync(user, subscribe, DateTime.Now, DateTime.Now.AddMonths(SubscribeMonthsDuration));
    }


    public async Task AddUserToSubscribeAsync(User user, Subscribe subscribe, DateTime startDate, DateTime endDate)
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