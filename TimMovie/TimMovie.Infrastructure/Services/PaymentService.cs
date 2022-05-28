using Microsoft.AspNetCore.Identity;
using TimMovie.Core.DTO.Payment;
using TimMovie.Core.Entities;
using TimMovie.Core.Interfaces;
using TimMovie.SharedKernel.Classes;

namespace TimMovie.Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private readonly ISubscribeService _userSubscribeService;
    private readonly UserManager<User> _userManager;

    public PaymentService(ISubscribeService userSubscribeService, UserManager<User> userManager)
    {
        this._userSubscribeService = userSubscribeService;
        _userManager = userManager;
    }
    
    public bool IsCardValid(CardDto card)
    {
        var date = DateTime.Now;
        return card.ExpirationYear >= date.Year &&
               (card.ExpirationYear != date.Year || card.ExpirationMonth >= date.Month);
    }

    public async Task<Result> PaySubscribeAsync(Guid? userId, Guid subscribeId, CardDto cardDto)
    {
        var userFromDb = await _userManager.FindByIdAsync(userId.ToString());
        if (userFromDb is null)
            return Result.Fail("данного пользователя не существует");
        var subscribeFromDb = _userSubscribeService.GetSubscribeById(subscribeId);
        if (subscribeFromDb is null)
            return Result.Fail("данной подписки не существует");
        if (!IsCardValid(cardDto)) 
            return Result.Fail("карта является невалидной");
        if(new Random().NextDouble() < 0.15)
            return Result.Fail("банк отклонил вашу покупку");
        await _userSubscribeService.AddUserToSubscribeAsync(userFromDb, subscribeFromDb);
        return Result.Ok();
    }
}