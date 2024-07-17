using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManagement.API.Entities.Configuration;

namespace UserManagement.API.Entities
{
    public class UserContext : IdentityDbContext<
     User,
     Role,
     int,
     IdentityUserClaim<int>,
     UserRole,
     IdentityUserLogin<int>,
     IdentityRoleClaim<int>,
     IdentityUserToken<int>>
    {

        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasMany(ur => ur.UserRoles).WithOne(u => u.User).HasForeignKey(ur => ur.UserId).IsRequired();
            modelBuilder.Entity<Role>().HasMany(ur => ur.UserRoles).WithOne(u => u.Role).HasForeignKey(ur => ur.RoleId).IsRequired();

            new UserEntityNamePropertyConfig().Configure(modelBuilder.Entity<User>());
            new UserEntityAddressPropertyConfig().Configure(modelBuilder.Entity<User>());

        }

    }

}
