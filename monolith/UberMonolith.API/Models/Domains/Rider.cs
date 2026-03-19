using UberMonolith.Models.Enums;

namespace UberMonolith.API.Models.Domains;

public class Rider
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required User User { get; set; } // navigation property
    public RiderStatus RiderStatus { get; set; }

}
