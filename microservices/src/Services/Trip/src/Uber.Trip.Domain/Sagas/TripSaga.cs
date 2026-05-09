using System;

public record TripRequested(Guid RiderId, Location PickupLocation, Location DropoffLocation);
public record TripAccepted(Guid TripId, Guid DriverId);
public record TripInProgress(Guid TripId);
public record TripCancelled(Guid TripId, string Reason);
public record TripStarted(Guid TripId);
public record TripCompleted(Guid TripId);






public class TripSaga
{
}
