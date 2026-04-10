using UberMonolith.API.Models.Domains;
using UberMonolith.API.Models.DTOs;

namespace UberMonolith.API.Repositories;

public interface IRideRepository
{
    public Task<Ride> RequestNewRide(Ride ride);
    public Task<Ride> GetRideById(Guid rideId);
    public Task<List<Ride>> GetRidesByRiderId(Guid riderId);
    
    public Task<Ride> AcceptRide(Ride ride);
    public Task<Ride> CompleteRide(Ride ride);
    public Task<Ride> CancelRide(Ride ride);
    public Task<List<NearbyDriverDto>> GetNearbyDrivers(double latitude,double longitude, double radiusKm);
}
