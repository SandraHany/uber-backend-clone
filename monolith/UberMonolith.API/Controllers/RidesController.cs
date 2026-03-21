using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UberMonolith.API.Models.Domains;
using UberMonolith.API.Models.DTOs;
using UberMonolith.API.Repositories;


namespace UberMonolith.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RidesController : ControllerBase
{
    private readonly IRideRepository _rideRepository;
    private readonly IMapper _mapper;
    public RidesController(IRideRepository rideRepository, IMapper mapper)
    {
        _rideRepository = rideRepository;
        _mapper = mapper;
    }
    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<ActionResult<RideDto>> GetRide(Guid id)
    {
        var ride = await _rideRepository.GetRideById(id);
        if (ride == null)
        {
            return NotFound();
        }
        return Ok(ride);
    }
    [HttpPost]
    public async Task<IActionResult> RequestNewRide(RequestRideDto rideRequestDto)
    {
        var rideModel = _mapper.Map<Ride>(rideRequestDto);
        var newRide = await _rideRepository.RequestNewRide(rideModel);
        var rideDto = _mapper.Map<RideDto>(newRide);
        return CreatedAtAction(nameof(GetRide), new { id = rideDto.Id }, rideDto);
    }
    [HttpGet]
    [Route("nearby-drivers")]
    public async Task<ActionResult<List<NearbyDriverDto>>> GetNearbyDrivers(double latitude, double longitude, int radiusKm = 3)
    {
        var nearbyDrivers = await _rideRepository.GetNearbyDrivers(latitude, longitude, radiusKm);
        return Ok(nearbyDrivers);
    }
}