using Domain.Models;
using Domain.Repository;
using Domain.Service;

namespace Infrastructure.Service
{
    public class BankMerchantInformationsService : IBankMerchantInformationsService
    {
        private readonly IBankMerchantInformationsRepository _bankMerchantInformationsRepository;

        public BankMerchantInformationsService(IBankMerchantInformationsRepository bankMerchantInformationsRepository)
        {
            _bankMerchantInformationsRepository = bankMerchantInformationsRepository;
        }

        public BankMerchantInformation GetByMerchantId(Guid merchantId)
        {
            return _bankMerchantInformationsRepository.GetByMerchantId(merchantId);
        }
    }
}
