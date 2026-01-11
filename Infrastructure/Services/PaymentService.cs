using Domain.Dtos;
using Domain.Models;
using Infrastructure.Clients;
using Infrastructure.Repositories;
using Infrastructure.Repository;
using Infrastructure.Services;

namespace Infrastructure.Service
{
    public class PaymentService
    {
        private readonly PaymentRepository _repository;
        private readonly PaymentFinalizationRepository _paymentFinalizationRepository;
        private readonly BankPaymentRequestService _bankPaymentRequestService;
        private readonly PspClient _pspClient;
        public PaymentService(PaymentRepository repository, PaymentFinalizationRepository paymentFinalizationRepository, BankPaymentRequestService bankPaymentRequestService, PspClient pspClient)
        {
            _repository = repository;
            _paymentFinalizationRepository = paymentFinalizationRepository;
            _bankPaymentRequestService = bankPaymentRequestService;
            _pspClient = pspClient;
        }

        public bool ValidatePaymentCard(PaymentCardDto dto)
        {
            var card = _repository.GetByPan(dto.Pan);

            if (card == null)
                return false;

            if (!ValidateCheckDigit(card.Pan))
                return false;

            if (card.SecurityCode != dto.SecurityCode)
                return false;

            if (card.CardholderName != dto.CardholderName)
                return false;

            if (card.ExpirationDate != dto.ExpirationDate)
                return false;

            if (card.ExpirationDate < DateOnly.FromDateTime(DateTime.UtcNow))
                return false;

            return true;
        }

        public static bool ValidateCheckDigit(String str)
        {
            var sum = 0;
            var shouldApplyDouble = true;
            for (var index = str.Length - 2; index >= 0; index--)
            {
                var currentDigit = (Int32)Char.GetNumericValue(str, index);
                if (shouldApplyDouble)
                {
                    if (currentDigit > 4)
                    {
                        sum += currentDigit * 2 - 9;
                    }
                    else
                    {
                        sum += currentDigit * 2;
                    }
                }
                else
                {
                    sum += currentDigit;
                }
                shouldApplyDouble = !shouldApplyDouble;
            }
            var checkDigit = 10 - (sum % 10);

            return Char.GetNumericValue(str[^1]) == checkDigit;
        }

        public bool ValidateBankPaymentRequest(BankPaymentRequestDto dto)
        {
            // TODO : IMPLEMENT

            return true;
        }

        public BankPaymentResponseDto CreateBankPaymentResponse(BankPaymentRequestDto request)
        {
            BankPaymentRequest bankPaymentRequest = new BankPaymentRequest()
            {
                MerchantId = request.MerchantId,
                Amount = request.Amount,
                Currency = request.Currency,
                Stan = request.Stan,
                PspTimestamp = request.PspTimestamp,
            };
            var saved = _bankPaymentRequestService.Create(bankPaymentRequest);
            Guid paymentId = saved.Id;
            var response = new BankPaymentResponseDto()
            {
                PaymentId = paymentId,
                PaymentUrl = "http://localhost:4202/payment/" + paymentId.ToString()
            };

            return response;
        }

        public bool ValidateMerchant(Guid merchantId)
        {
            var merchant = _repository.GetByMerchantId(merchantId);
            if (merchant == null)
                return false;
            return true;
        }

        public PaymentFinalizationRequestDto Pay(PaymentCardDto dto, Guid orderId)
        {
            var card = _repository.GetMatchingCard(dto.Pan, dto.SecurityCode, dto.CardholderName, dto.ExpirationDate);
            var status = Domain.Enums.Status.Success;

            if (card == null)
                status = Domain.Enums.Status.Error;

            if (card.ExpirationDate < DateOnly.FromDateTime(DateTime.UtcNow))
                status = Domain.Enums.Status.Fail;

            if (!ValidateCheckDigit(card.Pan))
                status = Domain.Enums.Status.Fail;

            var order = _bankPaymentRequestService.GetBankPaymentRequest(orderId);

            if (order == null)
                status = Domain.Enums.Status.Error;

            if (order.Amount > card.Amount)
                status = Domain.Enums.Status.Fail;

            if (status == Domain.Enums.Status.Success)
                card.Amount -= order.Amount;

            _repository.UpdateCard(card);
            _bankPaymentRequestService.UpdateStatus(orderId, status);

            PaymentFinalizationRequestDto request = new PaymentFinalizationRequestDto()
            {
                AcquirerTimestamp = DateTime.UtcNow,
                GlobalTransactionId = Guid.NewGuid(),
                Stan = order.Stan,
                Status = status
            };

            return request;
        }

        public async Task<PaymentFinalizationResponseDto> UpdateStatus(PaymentFinalizationRequestDto request)
        {
            PaymentFinalization finalization = new PaymentFinalization()
            {
                AcquirerTimestamp = request.AcquirerTimestamp,
                GlobalTransactionId = request.GlobalTransactionId,
                Stan = request.Stan,
                Status = request.Status
            };
            _paymentFinalizationRepository.CreatePaymentFinalization(finalization);
            var response = await _pspClient.FinalizeAsync(request);
            return response;
        }
    }
}
