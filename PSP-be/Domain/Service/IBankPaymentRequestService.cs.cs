using Domain.Models;

namespace Domain.Service
{
    public interface IBankPaymentRequestService
    {
        BankPaymentRequest Create(BankPaymentRequest request);
    }
}