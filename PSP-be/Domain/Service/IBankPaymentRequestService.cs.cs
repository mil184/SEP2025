using Domain.Dtos;

namespace Domain.Service
{
    public interface IBankPaymentRequestService
    {
        Task<BankPaymentResponseDto> Create(Guid orderId);
        Task<PaymentFinalizationResponseDto> Finalize(PaymentFinalizationRequestDto request);
    }
}