using Domain.Enums;

namespace Domain.Models
{
    // Request2 (Tabela2) - used to get PaymentId and PaymentUrl
    public class BankPaymentRequest
    {
        public Guid Id { get; set; }
        public Guid MerchantId { get; set; }
        public double Amount { get; set; }
        public Currency Currency { get; set; }
        public Guid Stan { get; set; }
        public DateTime PspTimestamp { get; set; }
        public Status Status { get; set; }
    }
}
