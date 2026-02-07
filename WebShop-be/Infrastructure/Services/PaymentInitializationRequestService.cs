using Domain.Dtos;
using Domain.Models;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class PaymentInitializationRequestService : IPaymentInitializationRequestService
    {
        private readonly IPaymentInitializationRequestRepository _repository;

        private readonly string _merchant_id;
        private readonly string _merchant_password;

        private readonly ILogger<PaymentInitializationRequestService> _logger;

        public PaymentInitializationRequestService(IPaymentInitializationRequestRepository repository, IConfiguration config, ILogger<PaymentInitializationRequestService> logger)
        {
            _repository = repository;
            _logger = logger;
            _merchant_id = config["Payment:MerchantId"]!;
            _merchant_password = config["Payment:MerchantPassword"]!;

        }

        public PaymentInitializationRequest Create(PaymentInitializationRequestDto dto)
        {
            _logger.LogInformation(
                "Payment init request create attempt. merchantId={MerchantIdRef} currency={Currency} amountMinor={Amount} orderRef={OrderRef}",
                SafeRefHelper.SafeRef(_merchant_id.ToString()),
                dto?.Currency,
                dto?.Amount,
                SafeRefHelper.SafeRef(dto?.MerchantOrderId.ToString()));

            if (dto == null)
            {
                _logger.LogWarning("Payment init request create failed: dto was null");
                throw new ArgumentNullException(nameof(dto));
            }

            // Validate minimal inputs without logging raw values
            if (dto.Amount <= 0)
            {
                _logger.LogWarning(
                    "Payment init request create failed: invalid amount. orderRef={OrderRef}",
                    SafeRefHelper.SafeRef(dto.MerchantOrderId.ToString()));
                throw new ArgumentException("Amount must be greater than 0.", nameof(dto.Amount));
            }

            if (string.IsNullOrWhiteSpace(dto.Currency.ToString()))
            {
                _logger.LogWarning(
                    "Payment init request create failed: currency was empty. orderRef={OrderRef}",
                    SafeRefHelper.SafeRef(dto.MerchantOrderId.ToString()));
                throw new ArgumentException("Currency is required.", nameof(dto.Currency));
            }

            if (string.IsNullOrWhiteSpace(dto.MerchantOrderId.ToString()))
            {
                _logger.LogWarning("Payment init request create failed: MerchantOrderId was empty");
                throw new ArgumentException("MerchantOrderId is required.", nameof(dto.MerchantOrderId));
            }

            try
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

                var created = _repository.Create(request);

                if (created == null)
                {
                    _logger.LogWarning(
                        "Payment init request create failed: repository returned null. reqId={RequestId} orderRef={OrderRef}",
                        request.Id,
                        SafeRefHelper.SafeRef(dto.MerchantOrderId.ToString()));
                    throw new InvalidOperationException("Failed to create payment initialization request.");
                }

                _logger.LogInformation(
                    "Payment init request create succeeded. reqId={RequestId} tsUtc={TimestampUtc} orderRef={OrderRef}",
                    created.Id,
                    created.MerchantTimestamp,
                    SafeRefHelper.SafeRef(dto.MerchantOrderId.ToString()));

                return created;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Payment init request create error. merchantId={MerchantSuffix} orderRef={OrderRef}",
                    SafeRefHelper.SafeRef(_merchant_id.ToString()),
                    SafeRefHelper.SafeRef(dto.MerchantOrderId.ToString()));
                throw;
            }
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

