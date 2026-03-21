using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UberMonolith.API.Models.Domains;
using UberMonolith.API.Repositories;
using UberMonolith.API.Models.DTOs;
using UberMonolith.API.Models.DTOs; 
namespace UberMonolith.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IMapper _mapper;
        public DriversController(IDriverRepository driverRepository, IMapper mapper)
        {
            _driverRepository = driverRepository;
            _mapper = mapper;
        }
        [HttpPut]
        public async Task<IActionResult> UpdateLocation(Guid driverId, double latitude, double longitude)
        {
            await _driverRepository.UpdateDriverLocation(driverId, latitude, longitude);
            return NoContent();
        }
        [HttpPost]
        public async Task<IActionResult> CreateDriver([FromBody] CreateDriverDto createDriverDto)
        {
            var driver = _mapper.Map<Driver>(createDriverDto);
            await _driverRepository.CreateDriverAsync(driver);
            return CreatedAtAction(nameof(GetDriver), new { id = driver.Id }, driver);
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetDriver(Guid id)
        {
            var driver = await _driverRepository.GetDriverByIdAsync(id);
            if (driver == null)
            {
                return NotFound();
            }
            var driverDto = _mapper.Map<DriverDto>(driver);
            return Ok(driverDto);
        }
    }
}
