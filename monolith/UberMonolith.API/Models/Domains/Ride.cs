using UberMonolith.API.Models.Enums;

namespace UberMonolith.API.Models.Domains;

public class Ride
{
    public Guid Id { get; set; }
    public Guid DriverId { get; set; }
    public Guid RiderId { get; set; }
    public required string  PickupLatitude { get; set; }
    public required string PickupLongitude { get; set; }
    public required string DropoffLatitude { get; set; }
    public required string DropoffLongitude { get; set; }
    public DateTime AcceptedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? CanceledAt { get; set; }
    public DateTime RequestedAt { get; set; }
    public string? CancellationReason { get; set; }
    public decimal? Fare { get; set; }
    public RideStatus RideStatus { get; set; }
    public double Distance {get;set;}
    public RideType RideType { get; set; }

}
