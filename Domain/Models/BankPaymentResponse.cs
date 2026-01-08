namespace Domain.Models
{
    public class BankPaymentResponse
    {
        public Guid Id { get; set; }
        public string PaymentUrl { get; set; }
        public Guid PaymentId { get; set; }
    }
}
