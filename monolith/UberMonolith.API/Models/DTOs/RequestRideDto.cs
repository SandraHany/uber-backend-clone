using UberMonolith.Models.Enums;

namespace UberMonolith.API.Models.DTOs;

public class RequestRideDto
{
    public Guid Id {get; set;}
    public Guid RiderId {get; set;}
    public required string PickupLatitude {get; set;}
    public required string PickupLongitude {get; set;}
    public required string DropoffLatitude {get; set;}
    public required string DropoffLongitude {get; set;}
    public RideType RideType { get; set; }

}
