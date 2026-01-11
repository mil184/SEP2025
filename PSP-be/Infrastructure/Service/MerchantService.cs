using Domain.Dtos;
using Domain.Models;
using Domain.Repository;
using Domain.Service;

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
            var merchantId = dto.MerchantId;
            var merchant = GetByMerchantId(merchantId);
            return merchant.MerchantPassword == dto.MerchantPassword;
        }

        public Merchant Create(MerchantRequest req)
        {
            Merchant merchant = new Merchant()
            {
                MerchantId = new Guid(),
                MerchantPassword = req.MerchantPassword,
                MerchantName = req.MerchantName,
                SuccessUrl = req.SuccessUrl,
                ErrorUrl = req.ErrorUrl,
                FailedUrl = req.FailedUrl

            };
            return _merchantRepository.Create(merchant);
        }
    }
}
