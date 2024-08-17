using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<History> Histories { get; set; }
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasMany(ur => ur.UserRoles).WithOne(u => u.User).HasForeignKey(ur => ur.UserId).IsRequired();
            modelBuilder.Entity<User>().HasMany(u => u.Histories).WithOne(h => h.User).HasForeignKey(h => h.UserId).IsRequired();
            modelBuilder.Entity<Role>().HasMany(ur => ur.UserRoles).WithOne(u => u.Role).HasForeignKey(ur => ur.RoleId).IsRequired();

            modelBuilder.Entity<History>().HasKey(h => h.Id);
            modelBuilder.Entity<History>().Property(h => h.Id).ValueGeneratedOnAdd();   

            modelBuilder.Entity<User>().OwnsOne(u => u.Name, n =>
             {
                 n.Property(n => n.FirstName).HasColumnName("FirstName");
                 n.Property(n => n.LastName).HasColumnName("LastName");
             });
            
            modelBuilder.Entity<User>().OwnsOne(u => u.Address, a =>
             {
                 a.Property(a => a.City).HasColumnName("City");
                 a.Property(a => a.StreetNumber).HasColumnName("StreetNumber");
                 a.Property(a => a.StreetName).HasColumnName("StreetName");
                 a.Property(a => a.ZipCode).HasColumnName("ZipCode");
                 a.OwnsOne(a => a.GeoLocation, a => {
                     a.Property(g => g.Latitude).HasColumnName("Latitude");
                     a.Property(g => g.Longitude).HasColumnName("Longitude");
                 });
             });
        }

    }

}
