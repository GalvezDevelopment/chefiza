using AutoMapper;
using ChefizaApi.Dtos;
using ChefizaApi.Entities;

namespace ChefizaApi.Profiles {
    public class UserProfile : Profile {
        public UserProfile() {
            CreateMap<CreateUserDto, User>();
            CreateMap<User, CreateUserDto>();
            CreateMap<User, UserListDto>();
        }
    }
}