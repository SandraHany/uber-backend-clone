namespace UberMonolith.API.Models.DTOs;

public record CreateDriverDto
{
    public Guid UserId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string VehicleModel { get; init; } = string.Empty;
    public string LicensePlate { get; init; } = string.Empty;
    public string VehicleMake {get;init;} = string.Empty;
    public string Colour {get;init;} = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
}