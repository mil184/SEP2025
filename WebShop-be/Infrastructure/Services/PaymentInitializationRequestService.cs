using Domain.Dtos;
using Domain.Models;
using Domain.Repositories;
using Domain.Services;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class PaymentInitializationRequestService : IPaymentInitializationRequestService
    {
        private readonly IPaymentInitializationRequestRepository _repository;

        private readonly string _merchant_id;
        private readonly string _merchant_password;

        public PaymentInitializationRequestService(IPaymentInitializationRequestRepository repository, IConfiguration config)
        {
            _repository = repository;
            _merchant_id = config["Payment:MerchantId"]!;
            _merchant_password = config["Payment:MerchantPassword"]!;

        }

        public PaymentInitializationRequest Create(PaymentInitializationRequestDto dto)
        {
            var request = new PaymentInitializationRequest
            {
                Id = Guid.NewGuid(),
                MerchantId = _merchant_id,
                Amount = dto.Amount,
                Currency = dto.Currency,
                MerchantOrderId = dto.MerchantOrderId,
                MerchantTimestamp = DateTime.UtcNow
            };

            return _repository.Create(request);
        }

        public void Delete(PaymentInitializationRequest request)
        {
            _repository.Delete(request);
        }

        public IEnumerable<PaymentInitializationRequest> GetAll()
        {
            return _repository.GetAll();
        }

        public PaymentInitializationRequest GetById(Guid id)
        {
            var req = _repository.GetById(id);
            if (req == null)
                throw new ArgumentException("No payment initialization request found with the specified id.");

            return req;
        }

    }
}

