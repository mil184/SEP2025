namespace Domain.Models
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public double PricePerDay { get; set; }
        public bool IsAvailable { get; set; }
    }
}
