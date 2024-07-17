using Microsoft.AspNetCore.Identity;

namespace UserManagement.API.Entities
{
    public class Role: IdentityRole<int>
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
