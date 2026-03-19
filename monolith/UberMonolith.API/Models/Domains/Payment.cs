using UberMonolith.Models.Enums;

namespace UberMonolith.API.Models.Domains;

public class Payment
{   
    public Guid Id { get; set; }
    public Guid TripId { get; set; }
    public Ride Ride { get; set; } = null!; // navigation property 
    public decimal Amount { get; set; }
    public DateTime PaidAt { get; set; }
    public DateTime UpdatedAt {get; set;}
    public PaymentStatus PaymentStatus {get; set;}
    public Guid TransactionId { get; set; }

}