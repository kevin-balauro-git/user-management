using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace UserManagement.API.Entities.Configuration
{
    public class UserEntityNamePropertyConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.OwnsOne(u => u.Name, n =>
            {
                n.Property(n => n.FirstName).HasColumnName("FirstName");
                n.Property(n => n.LastName).HasColumnName("LastName");
            });
        }
    }
}
