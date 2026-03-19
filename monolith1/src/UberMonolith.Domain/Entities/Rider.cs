namespace UberMonolith.Domain;

public class Rider
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public RiderStatus RiderStatus { get; set; }

}
