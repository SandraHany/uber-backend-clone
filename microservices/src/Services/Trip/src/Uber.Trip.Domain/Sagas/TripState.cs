using MassTransit;
using System;

/// <summary>
/// Summary description for Class1
/// </summary>
public class TripState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public DateTime? TripRequestedAt { get; set; }
}
