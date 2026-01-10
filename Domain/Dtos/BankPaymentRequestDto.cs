using Domain.Enums;

namespace Domain.Dtos
{
    public record BankPaymentRequestDto
    {
        public Guid MerchantId { get; set; }
        public double Amount { get; set; }
        public Currency Currency { get; set; }
        public Guid Stan { get; set; }
        public DateTime PspTimestamp { get; set; }
    }
}
