namespace Domain.Dtos
{
    public record PaymentCardDto
    {
        public string Pan { get; set; }
        public string SecurityCode { get; set; }
        public string CardholderName { get; set; }
        public DateOnly ExpirationDate { get; set; }
    }
}
