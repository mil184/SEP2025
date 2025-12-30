namespace Domain.Dtos
{
    public record ReservationDto
    {
        public Guid UserId { get; set; }
        public Guid VehicleId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
