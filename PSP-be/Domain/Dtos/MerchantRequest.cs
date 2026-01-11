namespace Domain.Dtos
{
    public record MerchantRequest
    {
        public string MerchantName { get; set; }
        public string MerchantPassword { get; set; }
        public string SuccessUrl { get; set; }
        public string FailedUrl { get; set; }
        public string ErrorUrl { get; set; }
    }
}
