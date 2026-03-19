using AutoMapper;
using UberMonolith.API.Models.Domains;
using UberMonolith.API.Models.DTOs;

namespace UberMonolith.API.Mappings;
 public class AutoMapperProfiles : Profile
 { 
    public AutoMapperProfiles()
    {
        CreateMap<Ride, RequestRideDto>().ReverseMap();
        CreateMap<Ride, RideDto>().ReverseMap();
         
    }
}
