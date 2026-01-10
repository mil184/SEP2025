using Domain.Models;

namespace Domain.Repository
{
    public interface IBankMerchantInformationsRepository
    {
        BankMerchantInformation GetByMerchantId(Guid merchantId);
    }
}
