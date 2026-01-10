using Domain.Models;

namespace Domain.Service
{
    public interface IBankMerchantInformationsService
    {
        BankMerchantInformation GetByMerchantId(Guid merchantId);
    }
}
