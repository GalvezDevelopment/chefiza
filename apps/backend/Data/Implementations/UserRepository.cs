using ChefizaApi.Context;
using ChefizaApi.Contracts;
using ChefizaApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChefizaApi.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
        }

        public async Task DeleteAll() {
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE User");
        }

        public async Task<IEnumerable<User>> GetAllAsync(bool allowTracking = true)
        {
            if (allowTracking) return await _context.Users.ToListAsync();
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User?> ReadAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> UpdateAsync(User user)
        {
            var userFound = await _context.Users.FindAsync(user.Email);
            if (userFound != null)
            {
                userFound.FirstName = user.FirstName;
                userFound.LastName = user.LastName;
                _context.Users.Update(userFound);
                return userFound;
            }
            return null;
        }

        public async Task<Auth?> LogIn(string email, string password)
        {
            var credentials_db = await _context.Authentications.Include(p => p.Profile).FirstOrDefaultAsync(p => p.Email == email);

            if (credentials_db != null)
            {
                return credentials_db;
            }

            return null;
        }

        public Task LogOut()
        {
            throw new NotImplementedException();
        }
    }
}