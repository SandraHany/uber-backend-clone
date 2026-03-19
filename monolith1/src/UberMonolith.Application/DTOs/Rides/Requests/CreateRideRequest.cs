using UberMonolith.Domain;

namespace UberMonolith.Application;

public record CreateRideRequest()
{
    public  Guid RiderId { get; init; }
    public LocationDto PickupLocation { get; init; }
    public LocationDto DropoffLocation { get; init; }
    public PaymentMethod PaymentMethod { get; init; }   
    public RideType RideType { get; init; }
}
