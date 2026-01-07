using Domain.Dtos;
using Domain.Models;

namespace Domain.Service
{
    public interface IMerchantService
    {
        Merchant? GetByMerchantId(Guid id);
        bool VerifyMerchant(PaymentInitializationRequestDto dto);
    }
}
