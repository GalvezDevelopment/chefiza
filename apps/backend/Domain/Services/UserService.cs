using AutoMapper;
using BCrypt.Net;
using ChefizaApi.Contracts;
using ChefizaApi.Dtos;
using ChefizaApi.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace ChefizaApi.Services
{
   public class UserService
   {
      private IUnitOfWork _unitOfWork { get; set; }
      private IMapper _mapper { get; set; }
      public UserService(IUnitOfWork unitOfWork, IMapper mapper)
      {
         _unitOfWork = unitOfWork;
         _mapper = mapper;
      }

      public async Task Add(CreateUserDto dto)
      {
         dto.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(dto.Password);
         User user = _mapper.Map<User>(dto);
         Auth credentials = _mapper.Map<Auth>(dto);
         user.Credentials = credentials;
         await _unitOfWork.Users.CreateAsync(user);
         await _unitOfWork.SaveChangesAsync();
      }

      public async Task<UserListDto?> findByEmail(string email)
      {
         var user = await _unitOfWork.Users.ReadAsync(email);
         if (user == null) return null;
         return _mapper.Map<UserListDto>(user);
      }

      public async Task<UserListDto?> Update(UpdateUserDto user)
      {
         var existingUser = await _unitOfWork.Users.ReadAsync(user.Email);
         if (existingUser == null) return null;
         existingUser.FirstName = user.FirstName;
         existingUser.LastName = user.LastName;
         existingUser.Email = user.Email;
         await _unitOfWork.SaveChangesAsync();
         return _mapper.Map<UserListDto>(existingUser);
      }

      public async Task<IEnumerable<UserListDto>> GetAll()
      {
         IEnumerable<User> users = await _unitOfWork.Users.GetAllAsync(allowTracking: false);
         return _mapper.Map<UserListDto[]>(users);
      }

      public async Task DeleteById(string email)
      {
         await _unitOfWork.Users.DeleteAsync(email);
         await _unitOfWork.SaveChangesAsync();
      }

      public async Task DeleteAll() {
         await _unitOfWork.Users.DeleteAll();
      }

      public async Task<UserListDto?> Login(LoginDto dto)
      {
         if (dto == null || dto.Email == null || dto.Password == null) return null;
         var credentials = await _unitOfWork.Users.LogIn(dto.Email, dto.Password);
         if (credentials == null) return null;
         var isValid = BCrypt.Net.BCrypt.EnhancedVerify(dto.Password, credentials.Password);
         if (isValid == false) return null;
         return _mapper.Map<UserListDto>(credentials.Profile);
      }

   }
}