using Domain.Dtos;
using Domain.Models;

namespace Domain.Services
{
    public interface IPaymentInitializationRequestService
    {
        PaymentInitializationRequest Create(PaymentInitializationRequestDto request);
        PaymentInitializationRequest GetById(Guid id);
    }
}
