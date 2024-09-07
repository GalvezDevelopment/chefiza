using ChefizaApi.Entities;

namespace ChefizaApi.Contracts {
    public interface IBaseRepository<Entity> where Entity : BaseEntity {
        Task CreateAsync(Entity user);
        Task<Entity?> ReadAsync(string id);
        Task<Entity?> UpdateAsync(Entity user);
        Task DeleteAsync(string id);
        Task<IEnumerable<Entity>> GetAllAsync(bool allowTracking = true);
        Task DeleteAll();
    }
}