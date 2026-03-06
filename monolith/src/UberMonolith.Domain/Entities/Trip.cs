namespace UberMonolith.Domain;

public class Trip
{
    public Guid Id { get; set; }
    public Guid DriverId { get; set; }
    public Guid RiderId { get; set; }
    public decimal PickupLatitude { get; set; }
    public decimal PickupLongitude { get; set; }
    public decimal DropoffLatitude { get; set; }
    public decimal DropoffLongitude { get; set; }
    public DateTime AcceptedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? CanceledAt { get; set; }
    public DateTime RequestedAt { get; set; }
    public string? CancellationReason { get; set; }
    public decimal Fare { get; set; }
    public TripStatus TripStatus { get; set; }
    public double Distance {get;set;}

}
