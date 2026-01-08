namespace Domain.Models
{
    public class PaymentCard
    {
        public Guid Id { get; set; }
        public string Pan { get; set; }
        public string SecurityCode { get; set; }
        public string CardholderName { get; set; }
        public DateOnly ExpirationDate { get; set; }
        public double Amount { get; set; }
    }
}