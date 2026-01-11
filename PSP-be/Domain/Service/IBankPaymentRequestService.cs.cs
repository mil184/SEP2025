using Domain.Dtos;

namespace Domain.Service
{
    public interface IBankPaymentRequestService
    {
        Task<BankPaymentResponseDto> Create(Guid orderId);
        PaymentFinalizationResponseDto Finalize(PaymentFinalizationRequestDto request);
    }
}