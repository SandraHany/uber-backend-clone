namespace UberMonolith.API;

public record RideRequestedEvent
{
    public Guid RiderId { get; init; }
    public Guid DriverId { get; init; }
    public DateTime RequestedAt { get; init; }
    public string PickupLatitude { get; init; }
    public string PickupLongitude { get; init; }
    public string DropoffLatitude { get; init; }
    public string DropoffLongitude { get; init; }
}
