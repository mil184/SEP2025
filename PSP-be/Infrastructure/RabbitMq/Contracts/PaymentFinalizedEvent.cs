using Domain.Enums;

namespace Infrastructure.RabbitMq.Contracts
{
    public class PaymentFinalizedEvent
    {
        public Guid OrderId { get; set; }
        public Status Status { get; set; }
    }
}
