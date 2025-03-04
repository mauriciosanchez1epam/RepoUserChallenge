using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    public class UserProfile : Profile
    {

        public UserProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => Helpers.Helpers.CalculateAge(src.DateOfBirth)))
                .ReverseMap();

    
        }

    }
}
