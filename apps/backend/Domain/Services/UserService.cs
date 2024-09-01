using AutoMapper;
using ChefizaApi.Contracts;
using ChefizaApi.Dtos;
using ChefizaApi.Entities;

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

        public async Task Add(CreateUserDto dto) {
            User user = _mapper.Map<User>(dto);
            await _unitOfWork.Users.CreateUserAsync(user);
            await _unitOfWork.SaveChangesAsync();
         }

         public async Task<UserListDto?> findByEmail(string email) {
            var user = await _unitOfWork.Users.GetUserAsync(email);
            if (user == null) return null;
            return _mapper.Map<UserListDto>(user);
         }

         public async Task<UserListDto?> Update(UpdateUserDto user) {
            var existingUser = await _unitOfWork.Users.GetUserAsync(user.Email);
            if (existingUser == null) return null;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<UserListDto>(existingUser);
         }

         public async Task<IEnumerable<UserListDto>> GetAll() {
            IEnumerable<User> users = await _unitOfWork.Users.GetAllUsersAsync(allowTracking: false);
            return _mapper.Map<UserListDto[]>(users);
         }

         public async Task DeleteById(string email) {
            await _unitOfWork.Users.DeleteUserAsync(email);
            await _unitOfWork.SaveChangesAsync();
         }

    }
}