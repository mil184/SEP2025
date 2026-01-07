using Domain.Enums;

namespace Domain.Dtos
{
    public record PaymentInitializationRequestDto
    {
        public string MerchantId { get; set; }
        public string MerchantPassword { get; set; }
        public double Amount { get; set; }
        public Currency Currency { get; set; }
        public Guid MerchantOrderId { get; set; }
        public DateTime MerchantTimestamp { get; set; }
    }
}
