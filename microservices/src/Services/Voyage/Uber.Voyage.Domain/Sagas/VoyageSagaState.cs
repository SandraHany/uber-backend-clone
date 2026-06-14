using System;
using System.Collections.Generic;
using System.Text;
using Uber.Shared.Primitives;
using Uber.Voyage.Domain.Entities.ValueObjects;
using Uber.Voyage.Domain.Enums;
using Uber.Voyage.Domain.Events;

namespace Uber.Voyage.Domain.Sagas;

public sealed class VoyageSagaState
{
    public Guid Id { get; init; }
    public Guid PassengerId { get; init; }
    public Guid? DriverId { get; private set; }
    public Location Pickup { get; private set; } = default!;
    public Location Dropoff { get; private set; } = default!;
    //public Fare Fare { get; private set; } = default!;
    public VoyageStatus Status { get; private set; }
    public VehicleType VehicleType { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }
    public DateTime? CompletedAtUtc { get; private set; }
    public DateTime? DriverSearchDeadline { get; set; }
    public string? FailureReason { get; set; }
    public int Version { get; set; }
}

