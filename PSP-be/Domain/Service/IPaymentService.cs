using Domain.Dtos;
using Domain.Models;

namespace Domain.Service
{
    public interface IPaymentService
    {
        PaymentInitializationRequest Create(PaymentInitializationRequestDto request);
    }
}
