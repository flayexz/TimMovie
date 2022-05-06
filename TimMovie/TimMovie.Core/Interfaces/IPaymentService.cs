using System.ComponentModel.DataAnnotations;
using TimMovie.Core.DTO.Payment;
using TimMovie.SharedKernel.Classes;

namespace TimMovie.Core.Interfaces;

public interface IPaymentService
{
    public bool IsCardValid(CardDto card);

    public Task<Result> PaySubscribeAsync(SubscribePaymentDto subscribePaymentDto, CardDto cardDto);
}