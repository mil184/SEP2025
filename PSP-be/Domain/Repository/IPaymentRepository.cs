using Domain.Models;

namespace Domain.Repository
{
    public interface IPaymentRepository
    {
        PaymentInitializationRequest CreatePaymentInitializationRequest(PaymentInitializationRequest request);
        BankPaymentRequest CreateBankPaymentRequest(BankPaymentRequest request);
    }
}
