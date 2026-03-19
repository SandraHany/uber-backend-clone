using UberMonolith.Models.Enums;
namespace UberMonolith.API.Models.Domains;

public class Driver
{
    public Guid Id {get; set;}
    public required User User {get; set;} // navigation property
    public Guid UserId { get; set; } 
    public DriverStatus DriverStatus {get; set;}

}
