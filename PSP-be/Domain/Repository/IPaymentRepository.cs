using Domain.Models;

namespace Domain.Repository
{
    public interface IPaymentRepository
    {
        PaymentInitializationRequest Get(Guid id);
        PaymentInitializationRequest CreatePaymentInitializationRequest(PaymentInitializationRequest request);
        BankPaymentRequest CreateBankPaymentRequest(BankPaymentRequest request);
    }
}
