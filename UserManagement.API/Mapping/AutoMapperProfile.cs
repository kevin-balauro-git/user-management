using AutoMapper;
using UserManagement.API.Dto;
using UserManagement.API.Entities;

namespace UserManagement.API.Mapping
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<Name, NameDto>();
            CreateMap<Address, AddressDto>();
            CreateMap<GeoLocation, GeoLocationDto>();
            CreateMap<UserDto, User>();
            CreateMap<NameDto, Name>();
            CreateMap<AddressDto, Address>();
            CreateMap<GeoLocationDto, GeoLocation>();
        }
    }
}
