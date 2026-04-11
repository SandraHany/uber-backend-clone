using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using UberMonolith.API.Infrastructure.Data;
using UberMonolith.API.Models.Domains;
using UberMonolith.API.Models.Enums;
using UberMonolith.API.Models.DTOs;
namespace UberMonolith.API.Repositories;

public class RideRepository : IRideRepository
{
    private readonly AppDbContext _context;
    private readonly IDatabase _redisDb;
    private readonly IDriverRepository _driverRepository;
    public RideRepository(AppDbContext context, IDatabase redisDb, IDriverRepository driverRepository) 
    {  
        _context = context;
        _redisDb = redisDb;
        _driverRepository = driverRepository;
    }

    public async Task<List<NearbyDriverDto>> GetNearbyDrivers(double latitude,double longitude, double radiusKm)
    {
        var geoResults = await _redisDb.GeoRadiusAsync(
            "drivers", 
            longitude,          
            latitude,           
            radiusKm,
            GeoUnit.Kilometers,
            count: 3,
            order: Order.Ascending,
            options: GeoRadiusOptions.WithDistance);
        var driverIds =  geoResults.Select(r => Guid.Parse(r.Member.ToString())).ToList();
        var drivers = await _driverRepository.GetDriversByIdsAsync(driverIds);
        var nearbyDrivers = geoResults
            .Join(
                drivers,
                geoResult=> Guid.Parse(geoResult.Member.ToString()),
                driver=> driver.Id,
                (geoResult, driver)  => new NearbyDriverDto
                {
                    Id = driver.Id,
                    DistanceFromRider = geoResult.Distance!.Value,
                    Name = driver.User.Name,
                    DriverStatus = driver.DriverStatus,
                    VehicleModel = driver.Vehicle.Model,
                    VehicleMake = driver.Vehicle.Make,
                    LicensePlate = driver.Vehicle.PlateNumber,
                    Colour = driver.Vehicle.Colour                    
                }
            ).ToList();
        return nearbyDrivers;
            
    }
    public async Task<Ride> RequestNewRide(Ride ride)
    {
        _context.Rides.Add(ride);
        await _context.SaveChangesAsync();
        DiagnosticsConfig.TripRequestsCounter.Add(
            1,
            new KeyValuePair<string, object?> ("rider.id",ride.RiderId)
        );
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