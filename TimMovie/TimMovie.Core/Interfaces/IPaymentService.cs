using TimMovie.Core.DTO.Payment;
using TimMovie.SharedKernel.Classes;

namespace TimMovie.Core.Interfaces;

public interface IPaymentService
{
    public bool IsCardValid(CardDto card);

    public Task<Result> PaySubscribeAsync(Guid? userId, Guid subscribeId, CardDto cardDto);
}