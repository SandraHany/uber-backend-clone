namespace UberMonolith.API.Models.DTOs;
using UberMonolith.API.Models.Enums;
public record DriverDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DriverStatus DriverStatus { get; init; }
    public string VehicleModel { get; init; } = string.Empty;
    public string LicensePlate { get; init; } = string.Empty;
    public string VehicleMake {get;init;} = string.Empty;
    public string Colour {get;init;} = string.Empty;
}