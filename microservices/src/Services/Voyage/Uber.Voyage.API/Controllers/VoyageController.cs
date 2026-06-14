using MediatR;
using Microsoft.AspNetCore.Mvc;
using Uber.ServiceDefaults.Http;
using Uber.Voyage.API.DTOs.Requests;
using Uber.Voyage.Application.Features.GetVoyage;
using Uber.Voyage.Application.Features.RequestTrip;

namespace Uber.Voyage.API.Controllers;
           
[Route("api/[controller]")]
[ApiController]
public class VoyageController(ISender sender) : ApiController
{
    [HttpPost("request-voyage")]
    public async Task<IActionResult> RequestVoyage([FromBody] RequestVoyageRequest req, CancellationToken ct)
    {
        var result = await sender.Send(new RequestVoyageCommand(
              req.PassengerId, req.PickupLat, req.PickupLng, req.PickupAddress,
              req.DropoffLat, req.DropoffLng, req.DropoffAddress, req.VehicleType), ct);
        return result.IsSuccess
          ? CreatedAtAction(nameof(GetVoyageById), new { id = result.Value }, result.Value)
          : Problem(result.Error.ToString());
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetVoyageById(Guid id, CancellationToken ct)
    {
        var result = await sender.Send(new GetVoyageByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Value) : Problem(result.Error);
    }

}
