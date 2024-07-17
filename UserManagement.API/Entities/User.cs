using Microsoft.AspNetCore.Identity;

namespace UserManagement.API.Entities
{
    public class User : IdentityUser<int>
    {
        public Name Name { get; set; } = new Name();
        public string Password { get; set; } = string.Empty;
        public Address Address { get; set; } = new Address();

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime DateUpdated { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
