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
        private readonly BankClient _bankClient;

        public BankPaymentRequestService(IBankPaymentRequestRepository bankPaymentRequestRepository, IPaymentService paymentService, IBankMerchantInformationsService merchantInformationsService, BankClient bankClient)
        {
            _bankPaymentRequestRepository = bankPaymentRequestRepository;
            _paymentService = paymentService;
            _merchantInformationsService = merchantInformationsService;
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
                Stan = Guid.NewGuid(),
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
    }
}

