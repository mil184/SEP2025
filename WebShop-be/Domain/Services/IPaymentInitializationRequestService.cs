using Domain.Dtos;
using Domain.Models;

namespace Domain.Services
{
    public interface IPaymentInitializationRequestService
    {
        PaymentInitializationRequest Create(PaymentInitializationRequestDto request);
        void Delete(PaymentInitializationRequest request);
        IEnumerable<PaymentInitializationRequest> GetAll();
        PaymentInitializationRequest GetById(Guid id);
    }
}
