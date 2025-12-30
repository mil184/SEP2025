namespace Domain.Dtos
{
    public record VehicleDto
    {
        public required string Brand { get; set; }
        public required string Model { get; set; }
        public required string Year { get; set; }
        public required double PricePerDay { get; set; }
        public required bool IsAvailable { get; set; }
    }
}
