using UberMonolith.API.Models.Domains;
using UberMonolith.API.Models.DTOs;

namespace UberMonolith.API;

public interface IRideService
{
    public Task<Ride> RequestNewRide(Ride request);
     
}
