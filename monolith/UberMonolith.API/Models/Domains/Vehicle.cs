
namespace UberMonolith.API.Models.Domains;

public class Vehicle
{
    public Guid Id {get; set;}
    public required string PlateNumber {get;set;}
    public required string Model {get;set;}
    public required string Make {get;set;}
    public required string Colour {get;set;}

}
