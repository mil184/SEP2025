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

        public PaymentInitializationRequest Create(PaymentInitializationRequestDto dto)
        {
            PaymentInitializationRequest request = new PaymentInitializationRequest()
            {
                MerchantId = dto.MerchantId,
                MerchantPassword = dto.MerchantPassword,
                Amount = dto.Amount,
                Currency = dto.Currency,
                MerchantOrderId = dto.MerchantOrderId,
                MerchantTimestamp = dto.MerchantTimestamp,
                PspOrderId = Guid.NewGuid()
            };

            return _repository.Create(request);
        }
    }
}
