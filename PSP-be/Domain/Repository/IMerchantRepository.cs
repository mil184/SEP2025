using Domain.Models;

namespace Domain.Repository
{
    public interface IMerchantRepository
    {
        Merchant? GetByMerchantId(Guid id);
    }
}
