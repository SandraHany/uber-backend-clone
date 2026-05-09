using System;
using System.Collections.Generic;
using System.Text;

namespace Uber.Trip.Domain.Entities;

public sealed class Trip
{
    public Guid Id { get; private set; }
    public Guid RiderId { get; private set; }
    public Guid DriverId { get; private set; }
    public Location PickupLocation { get; private set; }
    public Location DropoffLocation { get; private set; }
    public DateTime? StartAtUtc { get; private set; }
    public DateTime? CompletedAtUtc { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public TripStatus Status { get; private set; }
    
    public static Trip Create(Guid riderId, Location pickupLocation, Location dropoffLocation)
    {
        var trip = new Trip
        {
            Id = Guid.NewGuid(),
            RiderId = riderId,
            CreatedAtUtc = DateTime.UtcNow,
            PickupLocation = pickupLocation,
            DropoffLocation = dropoffLocation,
            Status = TripStatus.Requested
        };
        return trip;
    }


}

