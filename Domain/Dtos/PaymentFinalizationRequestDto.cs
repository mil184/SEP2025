using Domain.Enums;

namespace Domain.Dtos
{
    public class PaymentFinalizationRequestDto
    {
        public Status Status { get; set; }
        public Guid GlobalTransactionId { get; set; }
        public DateTime AcquirerTimestamp { get; set; }
        public Guid Stan { get; set; }
    }
}
