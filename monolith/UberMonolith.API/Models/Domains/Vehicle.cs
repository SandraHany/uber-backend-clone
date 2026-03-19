
namespace UberMonolith.API.Models.Domains;

public class Vehicle
{
    public Guid Id {get; set;}
    public Guid DriverId {get;set;}
    public required string PlateNumber {get;set;}
}
