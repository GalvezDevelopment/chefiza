using ChefizaApi.Entities;

namespace ChefizaApi.Contracts {
    public interface IBaseRepository<Entity> where Entity : BaseEntity {
        Task CreateUserAsync(Entity user);
        Task<Entity?> GetUserAsync(string id);
        Task<Entity?> UpdateUserAsync(Entity user);
        Task DeleteUserAsync(string id);
        Task<IEnumerable<Entity>> GetAllUsersAsync(bool allowTracking = true);
    }
}