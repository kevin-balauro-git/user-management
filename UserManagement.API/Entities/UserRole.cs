using Microsoft.AspNetCore.Identity;

namespace UserManagement.API.Entities
{
    public class UserRole: IdentityUserRole<int>
    { 
        public User User { get; set; }
        public Role Role { get; set; }
    }
}
