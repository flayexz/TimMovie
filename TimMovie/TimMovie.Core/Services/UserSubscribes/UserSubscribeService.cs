using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services.UserSubscribes;

public class UserSubscribeService
{
    private readonly IRepository<UserSubscribe> userSubscribeRepository;

    public UserSubscribeService(IRepository<UserSubscribe> userSubscribeRepository)
    {
        this.userSubscribeRepository = userSubscribeRepository;
    }

    public async Task AddUserToSubscribeAsync(User user, Subscribe subscribe)
    {
        await AddUserToSubscribeAsync(user, subscribe, DateTime.Now, DateTime.Now.AddMonths(1));
    }
    
    
    public async Task AddUserToSubscribeAsync(User user, Subscribe subscribe,DateTime startDate ,DateTime endDate)
    {
        var userSubscribe = new UserSubscribe
        {
            EndDate = endDate,
            StartDay = startDate,
            SubscribedUser = user,
            Subscribe = subscribe
        };
        await userSubscribeRepository.AddAsync(userSubscribe);
        await userSubscribeRepository.SaveChangesAsync();
    }
    
}