using Domain.Enums;

namespace Domain.Dtos
{
    public record PaymentInitializationRequestDto
    {
        public double Amount { get; set; }
        public Currency Currency { get; set; }
        public Guid MerchantOrderId { get; set; }
    }
}
