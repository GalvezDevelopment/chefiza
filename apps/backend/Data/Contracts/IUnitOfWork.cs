using ChefizaApi.Entities;

namespace ChefizaApi.Contracts
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        Task SaveChangesAsync();
    }
}