using TimMovie.Core.DTO.Payment;
using TimMovie.Core.Interfaces;
using TimMovie.Core.Services.UserSubscribes;
using TimMovie.SharedKernel.Classes;

namespace TimMovie.Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private readonly UserSubscribeService userSubscribeService;

    public PaymentService(UserSubscribeService userSubscribeService)
    {
        this.userSubscribeService = userSubscribeService;
    }
    public bool IsCardValid(CardDto card)
    {
        var date = DateTime.Now;
        return card.ExpirationYear >= date.Year &&
               (card.ExpirationYear != date.Year || card.ExpirationMonth >= date.Month);
    }

    public async Task<Result> PaySubscribeAsync(SubscribePaymentDto subscribePaymentDto, CardDto cardDto)
    {
        if (!IsCardValid(cardDto)) 
            return Result.Fail("карта является невалидной");
        var rnd = new Random();
        if(rnd.NextDouble() < 0.15)
            return Result.Fail("не удалось оплатить покупку");
        await userSubscribeService.AddUserToSubscribeAsync(subscribePaymentDto.User, subscribePaymentDto.Subscribe);
        return Result.Ok();
    }
}