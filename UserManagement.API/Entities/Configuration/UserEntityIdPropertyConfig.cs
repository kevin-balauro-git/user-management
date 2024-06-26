using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace UserManagement.API.Entities.Configuration
{
    public class UserEntityIdPropertyConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.HasAlternateKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnName("ID");
        }
    }
}
