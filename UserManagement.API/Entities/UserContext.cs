using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManagement.API.Entities.Configuration;

namespace UserManagement.API.Entities
{
    public class UserContext : IdentityDbContext<AccessUser>
    {
        public DbSet<User> Users { get; set; }
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");

            new UserEntityUuidPropertyConfig().Configure(modelBuilder.Entity<User>());
            new UserEntityIdPropertyConfig().Configure(modelBuilder.Entity<User>());
            new UserEntityNamePropertyConfig().Configure(modelBuilder.Entity<User>());
            new UserEntityAddressPropertyConfig().Configure(modelBuilder.Entity<User>());

        }

    }
}
