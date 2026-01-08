using Domain.Dtos;
using Domain.Models;

namespace Domain.Service
{
    public interface IPaymentService
    {
        PaymentInitializationRequest CreatePaymentInitializationRequest(PaymentInitializationRequestDto request);
        BankPaymentRequest CreateBankPaymentRequest(BankPaymentRequestDto request);
    }
}
