using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace UserManagement.API.Entities.Configuration
{
    public class UserEntityUuidPropertyConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> uuidBuilder)
        {
            uuidBuilder.HasKey(u => u.Uuid);
            uuidBuilder.Property(u => u.Uuid).HasValueGenerator<GuidValueGenerator>();
            uuidBuilder.Property(u => u.Uuid).HasColumnName("UUID");
        }
    }
}
