using Domain.Models;

namespace Domain.Repositories
{
    public interface IPaymentInitializationRequestRepository
    {
        PaymentInitializationRequest Create(PaymentInitializationRequest request);
        void Update(PaymentInitializationRequest request);
        void Delete(PaymentInitializationRequest request);
        IEnumerable<PaymentInitializationRequest> GetAll();
        PaymentInitializationRequest? GetById(Guid id);
    }
}
