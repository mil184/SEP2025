namespace Domain.Dtos
{
    public record PaymentInitializationResponseDto
    {
        public Guid PspOrderId { get; set; }
        public string RedirectUrl { get; set; }
    }
}
