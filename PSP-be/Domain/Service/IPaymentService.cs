using Domain.Dtos;
using Domain.Models;

namespace Domain.Service
{
    public interface IPaymentService
    {
        PaymentInitializationRequest Get(Guid id);
        PaymentInitializationRequest CreatePaymentInitializationRequest(PaymentInitializationRequestDto request);
        BankPaymentRequest CreateBankPaymentRequest(BankPaymentRequestDto request);
    }
}
