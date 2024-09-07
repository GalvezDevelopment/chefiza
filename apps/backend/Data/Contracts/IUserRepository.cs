using ChefizaApi.Entities;

namespace ChefizaApi.Contracts {
    public interface IUserRepository : IBaseRepository<User> {
        Task<Auth?> LogIn(string email, string password);
        Task LogOut();
    }
}