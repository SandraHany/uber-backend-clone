namespace UberMonolith.API.Models.DTOs;
using UberMonolith.API.Models.Domains;
using UberMonolith.API.Models.Enums;

public record NearbyDriverDto
{
    public Guid Id { get; init; }
    public double DistanceFromRider { get; init; }
    public string Name { get; init; }

    public DriverStatus DriverStatus { get; init; }
    public string VehicleModel { get; init; } = string.Empty;
    public string LicensePlate { get; init; } = string.Empty;
    public string VehicleMake {get;init;} = string.Empty;
    public string Colour {get;init;} = string.Empty;
}