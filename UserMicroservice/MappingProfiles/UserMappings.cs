using AutoMapper;
using SharedMicroservice.DTO;
using UserMicroservice.Models;

namespace UserMicroservice.MappingProfiles
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserIndexDTO>().ReverseMap();
            CreateMap<User, UserUpdateDTO>().ReverseMap();
            CreateMap<User, UserAvailableBidDTO>().ReverseMap();            
        }
    }
}
