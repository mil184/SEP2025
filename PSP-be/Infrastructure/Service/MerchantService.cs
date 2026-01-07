using Domain.Dtos;
using Domain.Models;
using Domain.Repository;
using Domain.Service;
using Infrastructure.Helpers;

namespace Infrastructure.Service
{
    public class MerchantService : IMerchantService
    {
        private readonly IMerchantRepository _merchantRepository;
        public MerchantService(IMerchantRepository merchantRepository)
        {
            _merchantRepository = merchantRepository;
        }

        public Merchant? GetByMerchantId(Guid id)
        {
            return _merchantRepository.GetByMerchantId(id);
        }

        public bool VerifyMerchant(PaymentInitializationRequestDto dto)
        {
            var merchantId = GuidHelper.GetGuidFromString(dto.MerchantId);
            var merchant = GetByMerchantId(merchantId);
            return merchant.MerchantPassword == dto.MerchantPassword;
        }
    }
}
