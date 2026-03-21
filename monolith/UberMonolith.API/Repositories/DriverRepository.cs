using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using UberMonolith.API.Infrastructure.Data;
using UberMonolith.API.Models.Domains;

namespace UberMonolith.API.Repositories;

public class DriverRepository : IDriverRepository
{
    private readonly IDatabase _redisDb;
    private readonly AppDbContext _context;
    public DriverRepository(IDatabase redisDb, AppDbContext context)
    {
        _redisDb = redisDb;
        _context = context;
    }

    public async Task UpdateDriverLocation(Guid driverId, double latitude, double longitude)
    {
        await _redisDb.GeoAddAsync("drivers", new GeoEntry(longitude, latitude, driverId.ToString()));
    }
    public async Task<List<Driver>> GetDriversByIdsAsync(List<Guid> driverIds)
    {
       return  await _context.Drivers.Where(x=> driverIds.Contains(x.Id)).Include(d => d.Vehicle).Include(d => d.User).ToListAsync();

    }
    public async Task<Driver?> GetDriverByIdAsync(Guid driverId)
    {
       return  await _context.Drivers.Include(d => d.Vehicle).Include(d => d.User).FirstOrDefaultAsync(d => d.Id == driverId);

    }
    public async Task CreateDriverAsync(Driver driver)
    {
        _context.Drivers.Add(driver);
        await _context.SaveChangesAsync();
    }
}
