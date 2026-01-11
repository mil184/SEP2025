using Domain.Enums;

namespace Domain.Models
{
    public class PaymentFinalization
    {
        public Guid Id { get; set; }
        public DateTime AcquirerTimestamp { get; set; }
        public Status Status { get; set; }
        public Guid GlobalTransactionId { get; set; }
        public Guid Stan { get; set; }
    }
}
