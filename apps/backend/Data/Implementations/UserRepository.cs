using ChefizaApi.Context;
using ChefizaApi.Contracts;
using ChefizaApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChefizaApi.Implementations
{
    public class UserRepository : IBaseRepository<User>
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null) {
                _context.Users.Remove(user);
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(bool allowTracking = true)
        {
            if (allowTracking) return await _context.Users.ToListAsync();
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User?> GetUserAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> UpdateUserAsync(User user)
        {
            var userFound = await _context.Users.FindAsync(user.Email);
            if (userFound != null) {
                userFound.FirstName = user.FirstName;
                userFound.LastName = user.LastName;
                _context.Users.Update(userFound);
                return userFound;
            }
            return null;
        }
    }
}