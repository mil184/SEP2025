using Domain.Dtos;
using Domain.Models;
using Domain.Repository;
using Domain.Service;

namespace Infrastructure.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repository;
        public PaymentService(IPaymentRepository repository)
        {
            _repository = repository;
        }

        public PaymentInitializationRequest CreatePaymentInitializationRequest(PaymentInitializationRequestDto dto)
        {
            PaymentInitializationRequest request = new PaymentInitializationRequest()
            {
                MerchantId = dto.MerchantId,
                Amount = dto.Amount,
                Currency = dto.Currency,
                MerchantOrderId = dto.MerchantOrderId,
                MerchantTimestamp = dto.MerchantTimestamp,
                PspOrderId = Guid.NewGuid()
            };

            return _repository.CreatePaymentInitializationRequest(request);
        }

        public BankPaymentRequest CreateBankPaymentRequest(BankPaymentRequestDto dto)
        {
            BankPaymentRequest request = new BankPaymentRequest()
            {
                MerchantId = dto.MerchantId,
                Amount = dto.Amount,
                Currency = dto.Currency,
                Stan = Guid.NewGuid(),
                PspTimestamp = DateTime.Now.ToUniversalTime()
            };

            return _repository.CreateBankPaymentRequest(request);
        }
    }
}
