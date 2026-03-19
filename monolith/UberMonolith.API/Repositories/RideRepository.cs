using Microsoft.EntityFrameworkCore;
using UberMonolith.API.Infrastructure.Data;
using UberMonolith.API.Models.Domains;
using UberMonolith.Models.Enums;
namespace UberMonolith.API.Repositories;

public class RideRepository : IRideRepository
{
    public readonly AppDbContext _context;
    public RideRepository(AppDbContext context) 
    {  
        _context = context;
    }
    public async Task<Ride> RequestNewRide(Ride ride)
    {
        _context.Rides.Add(ride);
        await _context.SaveChangesAsync();
        return ride;
    }

    public async Task<Ride?> GetRideById(Guid rideId)
    {
        return await _context.Rides.FindAsync(rideId);
    }

    public async Task<Ride> UpdateRide(Ride ride)
    {
        _context.Rides.Update(ride);
        await _context.SaveChangesAsync();
        return ride;
    }

    public async Task<List<Ride>> GetRidesByRiderId(Guid riderId)
    {
        return await _context.Rides.Where(r => r.RiderId == riderId).ToListAsync<Ride>();
    }

    public async Task<Ride> AcceptRide(Ride ride)
    {
       var rideToUpdate = await _context.Rides.FindAsync(ride.Id);
        if (rideToUpdate == null)
        {
            return null;
        }
        rideToUpdate.RideStatus = RideStatus.Accepted;
        await _context.SaveChangesAsync();
        return ride;
    }

    public async Task<Ride> CompleteRide(Ride ride)
    {
        var rideToUpdate = await _context.Rides.FindAsync(ride.Id);
        if (rideToUpdate == null)
        {
            return null;
        }
        rideToUpdate.RideStatus = RideStatus.Completed;
        await _context.SaveChangesAsync();
        return ride;
    }

    public async Task<Ride> CancelRide(Ride ride)
    {
        var rideToUpdate = await _context.Rides.FindAsync(ride.Id);
        if (rideToUpdate == null)
        {
            return null;
        }
        rideToUpdate.RideStatus = RideStatus.Cancelled;
        await _context.SaveChangesAsync();
        return ride;
    }
}