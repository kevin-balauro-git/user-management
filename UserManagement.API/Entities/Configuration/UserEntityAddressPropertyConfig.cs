using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace UserManagement.API.Entities.Configuration
{
    public class UserEntityAddressPropertyConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.OwnsOne(u => u.Address, a =>
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
