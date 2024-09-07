using ChefizaApi.Context;
using ChefizaApi.Contracts;
using ChefizaApi.Entities;

namespace ChefizaApi.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _context;
        public IUserRepository Users { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}