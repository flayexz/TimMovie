using TimMovie.Core.DTO.Subscribes;
using TimMovie.Core.Entities;

namespace TimMovie.Core.Interfaces;

public interface ISubscribeService
{
    public IEnumerable<SubscribeDto> GetSubscribesByNamePart(string? namePart, int take, int skip);

    public IEnumerable<UserSubscribeDto> GetAllActiveUserSubscribes(Guid userId);

    public Task AddUserToSubscribeAsync(User user, Subscribe subscribe);

    public Task AddUserToSubscribeAsync(User user, Subscribe subscribe, DateTime startDate, DateTime endDate);
}