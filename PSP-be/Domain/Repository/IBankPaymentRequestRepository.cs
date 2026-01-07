using Domain.Models;

namespace Domain.Repository
{
    public interface IBankPaymentRequestRepository
    {
        BankPaymentRequest Create(BankPaymentRequest request);
    }
}
