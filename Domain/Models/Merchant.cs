namespace Domain.Models
{
    public class Merchant
    {
        public Guid Id { get; set; }
        public Guid MerchantId { get; set; }
        public string MerchantPassword { get; set; }
    }
}
