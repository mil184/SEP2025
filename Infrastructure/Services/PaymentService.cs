using Domain.Dtos;
using Domain.Models;
using Infrastructure.Helpers;
using Infrastructure.Repository;

namespace Infrastructure.Service
{
    public class PaymentService
    {
        private readonly PaymentRepository _repository;
        public PaymentService(PaymentRepository repository)
        {
            _repository = repository;
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

        public BankPaymentResponse CreateBankPaymentResponse()
        {
            var bankPaymentResponse = new BankPaymentResponse()
            {
                PaymentId = Guid.NewGuid(),
                PaymentUrl = "https://www.instagram.com" // TODO : FIGURE OUT CORRECT URL
            };

            return _repository.CreateBankPaymentResponse(bankPaymentResponse);
        }

        public bool ValidateMerchant(string merchantId)
        {
            var merchantGuid = GuidHelper.GetGuidFromString(merchantId);
            var merchant = _repository.GetByMerchantId(merchantGuid);
            if (merchant == null)
                return false;
            return true;
        }
    }
}
