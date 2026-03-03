namespace UberMonolith.Domain;

public class Driver
{
    public Guid Id {get; set;}
    public User User {get; set;}
    public Guid UserId { get; set; } 
    public Enum DriverStatus {get; set;}

}
