using Domain.Dtos;
using Domain.Models;
using Domain.Repository;
using Domain.Service;
using Infrastructure.Clients;

namespace Infrastructure.Service
{
    public class BankPaymentRequestService : IBankPaymentRequestService
    {
        private readonly IBankPaymentRequestRepository _bankPaymentRequestRepository;
        private readonly IPaymentService _paymentService;
        private readonly IBankMerchantInformationsService _merchantInformationsService;
        private readonly IMerchantService _merchantService;
        private readonly BankClient _bankClient;

        public BankPaymentRequestService(IBankPaymentRequestRepository bankPaymentRequestRepository, IPaymentService paymentService, IBankMerchantInformationsService merchantInformationsService, IMerchantService merchantService, BankClient bankClient)
        {
            _bankPaymentRequestRepository = bankPaymentRequestRepository;
            _paymentService = paymentService;
            _merchantInformationsService = merchantInformationsService;
            _merchantService = merchantService;
            _bankClient = bankClient;
        }

        public async Task<BankPaymentResponseDto> Create(Guid orderId)
        {
            var payment = _paymentService.Get(orderId);
            Guid merchantId = payment.MerchantId;
            Guid bankMerchantId = _merchantInformationsService.GetByMerchantId(merchantId).BankMerchantId;
            var toSave = new BankPaymentRequest()
            {
                MerchantId = bankMerchantId,
                Amount = payment.Amount,
                Currency = payment.Currency,
                Stan = payment.PspOrderId,
                PspTimestamp = DateTime.UtcNow
            };
            var saved = _bankPaymentRequestRepository.Create(toSave);
            var request = new BankPaymentRequestDto()
            {
                MerchantId = bankMerchantId,
                Amount = payment.Amount,
                Currency = payment.Currency,
                Stan = toSave.Stan,
                PspTimestamp = toSave.PspTimestamp
            };
            var response = await _bankClient.GetRedirectAsync(request);
            return response;
        }

        public PaymentFinalizationResponseDto Finalize(PaymentFinalizationRequestDto request)
        {
            // TODO: save request?
            Guid stan = request.Stan;
            var payment = _paymentService.Get(stan);
            Guid merchantId = payment.MerchantId;
            var merchant = _merchantService.GetByMerchantId(merchantId);

            if (request.Status == Domain.Enums.Status.Success)
            {
                return new PaymentFinalizationResponseDto() { RedirectUrl = merchant.SuccessUrl };
            }
            else if (request.Status == Domain.Enums.Status.Fail)
            {
                return new PaymentFinalizationResponseDto() { RedirectUrl = merchant.FailedUrl };
            }
            else
            {
                return new PaymentFinalizationResponseDto() { RedirectUrl = merchant.ErrorUrl };
            }
        }
    }
}

