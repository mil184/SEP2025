using Domain.Dtos;
using Domain.Models;
using Domain.Repository;
using Domain.Service;
using Infrastructure.Clients;
using Infrastructure.RabbitMq;
using Infrastructure.RabbitMq.Contracts;

namespace Infrastructure.Service
{
    public class BankPaymentRequestService : IBankPaymentRequestService
    {
        private readonly IBankPaymentRequestRepository _bankPaymentRequestRepository;
        private readonly IPaymentService _paymentService;
        private readonly IBankMerchantInformationsService _merchantInformationsService;
        private readonly IMerchantService _merchantService;
        private readonly IRabbitMQPublisher<PaymentFinalizedEvent> _publisher;
        private readonly BankClient _bankClient;

        public BankPaymentRequestService(IBankPaymentRequestRepository bankPaymentRequestRepository, IPaymentService paymentService, IBankMerchantInformationsService merchantInformationsService, IMerchantService merchantService, IRabbitMQPublisher<PaymentFinalizedEvent> publisher, BankClient bankClient)
        {
            _publisher = publisher;
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

        public async Task<BankPaymentResponseDto> CreateQR(Guid orderId)
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
            var response = await _bankClient.GetRedirectQRAsync(request);
            return response;
        }

        public async Task<PaymentFinalizationResponseDto> Finalize(PaymentFinalizationRequestDto request)
        {
            // TODO: save request?
            Guid stan = request.Stan;
            var payment = _paymentService.Get(stan);
            Guid merchantId = payment.MerchantId;
            var merchant = _merchantService.GetByMerchantId(merchantId);

            string redirectUrl =
                request.Status == Domain.Enums.Status.Success ? merchant.SuccessUrl :
                request.Status == Domain.Enums.Status.Fail ? merchant.FailedUrl :
                                                               merchant.ErrorUrl;

            // Build the MQ event
            var evt = new PaymentFinalizedEvent
            {
                OrderId = payment.MerchantOrderId,
                Status = request.Status
            };

            await _publisher.PublishMessageAsync(evt, queueName: "payment.finalized");

            return new PaymentFinalizationResponseDto { RedirectUrl = redirectUrl };
        }
    }
}

