using AutoMapper;
using ChefizaApi.Dtos;
using ChefizaApi.Entities;

namespace ChefizaApi.Profiles {
    public class UserProfile : Profile {
        public UserProfile() {
            CreateMap<CreateUserDto, User>();
            CreateMap<CreateUserDto, Auth>();
            CreateMap<User, UserListDto>();
            CreateMap<User, Auth>();
            CreateMap<LoginDto, Auth>();
        }
    }
}