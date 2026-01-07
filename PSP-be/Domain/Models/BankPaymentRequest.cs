using Domain.Enums;

namespace Domain.Models
{
    public class BankPaymentRequest
    {
        public Guid Id { get; set; }
        public string MerchantId { get; set; }
        public double Amount { get; set; }
        public Currency Currency { get; set; }
        public string Stan { get; set; }
        public DateTime PspTimestamp { get; set; }
    }
}
