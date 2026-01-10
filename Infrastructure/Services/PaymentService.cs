using Domain.Dtos;
using Domain.Models;
using Infrastructure.Repository;
using Infrastructure.Services;

namespace Infrastructure.Service
{
    public class PaymentService
    {
        private readonly PaymentRepository _repository;
        private readonly BankPaymentRequestService _bankPaymentRequestService;
        public PaymentService(PaymentRepository repository, BankPaymentRequestService bankPaymentRequestService)
        {
            _repository = repository;
            _bankPaymentRequestService = bankPaymentRequestService;
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
                PaymentUrl = "http://localhost:4202/" + paymentId.ToString()
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
    }
}
