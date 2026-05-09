using MediatR;
using Microsoft.AspNetCore.Mvc;
using Uber.Trip.Application.Features.RequestTrip;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Uber.Trip.API.Controllers;

public class RequestTripRequest
{
    public Guid RiderId { get; set; }
    public Location pickupLocation{  get; set; }
    public Location dropoffLocation { get; set; }
}

[Route("api/[controller]")]
[ApiController]
public sealed class TripController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> RequestTrip(RequestTripRequest request, CancellationToken ct )
    {
        var result = await sender.Send(new RequestTripCommand(
            request.RiderId,
            request.pickupLocation,
            request.dropoffLocation
            ),ct);
        return Ok();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTrip(Guid id, CancellationToken ct)
    {
        // var result = await sender.Send(new GetTripQuery(id), ct);
        // var result = new Result
        return Ok();
    }
}
