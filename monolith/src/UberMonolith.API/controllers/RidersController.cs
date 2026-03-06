using Microsoft.AspNetCore.Mvc;
using UberMonolith.Domain;
using UberMonolith.Infrastructure;
namespace UberMonolith.API;

[Route("api/[controller]")]
[ApiController]
public class RidersController : ControllerBase
{
    private readonly AppDbContext _context;

    public RidersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Riders
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Rider>>> GetRiders()
    {
        return await _context.Riders.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Rider>> CreateRider(Rider rider)
    {
        _context.Riders.Add(rider);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRiders), new { id = rider.Id }, rider);      
    }

}