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
        CreateMap<Driver, DriverDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.VehicleModel, opt => opt.MapFrom(src => src.Vehicle.Model))
            .ForMember(dest => dest.VehicleMake, opt => opt.MapFrom(src => src.Vehicle.Make))
            .ForMember(dest => dest.LicensePlate, opt => opt.MapFrom(src => src.Vehicle.PlateNumber))
            .ForMember(dest => dest.Colour, opt => opt.MapFrom(src => src.Vehicle.Colour));

        CreateMap<CreateDriverDto, Driver>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => new User
            {
                Name = src.Name,
                Email = src.Email,
                Password = src.Password,
                PhoneNumber = src.PhoneNumber,
                Role = Models.Enums.Role.Driver
            }))
            .ForMember(dest => dest.Vehicle, opt => opt.MapFrom(src => new Vehicle
            {
                Model = src.VehicleModel,
                Make = src.VehicleMake,
                PlateNumber = src.LicensePlate,
                Colour = src.Colour
            }));
        
    }
}
