using ChefizaApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChefizaApi.Context {
    public class AppDbContext : DbContext {
        public AppDbContext(DbContextOptions options): base(options) {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Auth> Authentications { get; set; }
    }
}