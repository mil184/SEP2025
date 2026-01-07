using Domain.Enums;

namespace PSP.Dtos
{
    public record BankPaymentRequestDto
    {
        public string MerchantId { get; set; }
        public double Amount { get; set; }
        public Currency Currency { get; set; }
        public string Stan { get; set; }
    }
}
