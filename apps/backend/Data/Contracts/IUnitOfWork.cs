using ChefizaApi.Entities;

namespace ChefizaApi.Contracts
{
    public interface IUnitOfWork
    {
        IBaseRepository<User> Users { get; }
        Task SaveChangesAsync();
    }
}