using Microsoft.AspNetCore.Mvc;

namespace UberMonolith.API;

[Route("api/[controller]")]
[ApiController]

public class RidesController : ControllerBase
{
     private readonly IRiderRepository _riderRepository;

}
{
  "pickupLocation": {
    "latitude": 40.7128,
    "longitude": -74.006,
    "address": "123 Main St, New York, NY 10001",
    "city": "New York",
    "state": "NY",
    "country": "USA",
    "postalCode": "10001"
  },
  "dropoffLocation": {
    "latitude": 40.7128,
    "longitude": -74.006,
    "address": "123 Main St, New York, NY 10001",
    "city": "New York",
    "state": "NY",
    "country": "USA",
    "postalCode": "10001"
  },
  "rideType": "STANDARD",
  "scheduledTime": "2024-03-20T15:30:00Z",
  "passengerCount": 1,
  "notes": "Please call when arrived",
  "paymentMethod": "CARD"
}