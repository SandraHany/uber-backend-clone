namespace UberMonolith.Domain;

public class Payment
{
    public Guid Id { get; set; }
    public Guid TripId { get; set; }
    public Trip Trip { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTime PaidAt { get; set; }
    public DateTime UpdatedAt {get; set;}
    public PaymentStatus PaymentStatus {get; set;}
    public Guid TransactionId { get; set; }

}