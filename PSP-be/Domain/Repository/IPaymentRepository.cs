using Domain.Models;

namespace Domain.Repository
{
    public interface IPaymentRepository
    {
        PaymentInitializationRequest Create(PaymentInitializationRequest request);
    }
}
