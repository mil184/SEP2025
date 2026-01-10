using Domain.Enums;

namespace Domain.Models
{
    public class PaymentInitializationRequest
    {
        public Guid Id { get; set; }
        public Guid MerchantId { get; set; }
        public double Amount { get; set; }
        public Currency Currency { get; set; }
        public Guid MerchantOrderId { get; set; }
        public DateTime MerchantTimestamp { get; set; }
        public Guid PspOrderId { get; set; }
    }
}
