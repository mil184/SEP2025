using Domain.Dtos;
using Domain.Models;
using Infrastructure.Clients;
using Infrastructure.Repositories;
using Infrastructure.Repository;
using Infrastructure.Services;
using QRCoder;
using System.Globalization;


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

        public BankPaymentResponseDto CreateBankPaymentQRResponse(BankPaymentRequestDto request)
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
                PaymentUrl = "http://localhost:4202/payment/qr/" + paymentId.ToString()
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

        public double GetAmount(Guid orderId)
        {
            return _bankPaymentRequestService.GetBankPaymentRequest(orderId).Amount;
        }

        public byte[] GenerateQrPngForOrder(Guid orderId)
        {
            var paymentRequest = _bankPaymentRequestService.GetBankPaymentRequest(orderId);
            if (paymentRequest == null)
                throw new KeyNotFoundException($"Payment request not found for id: {orderId}");

            var currencyCode = paymentRequest.Currency.ToString();

            var amountRounded = Math.Round(paymentRequest.Amount, 2, MidpointRounding.AwayFromZero);
            var amountFormatted = amountRounded.ToString("0.00", CultureInfo.InvariantCulture).Replace('.', ',');

            var iField = $"{currencyCode}{amountFormatted}";

            var payload = $"K:PR|V:01|C:1|R:105000000000000029|N:Webshop|I:{iField}|SF:221";

            using var generator = new QRCodeGenerator();
            using var data = generator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);

            var pngQr = new PngByteQRCode(data);
            return pngQr.GetGraphic(pixelsPerModule: 10);
        }
    }
}
