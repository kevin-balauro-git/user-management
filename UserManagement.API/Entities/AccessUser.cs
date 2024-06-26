using Microsoft.AspNetCore.Identity;

namespace UserManagement.API.Entities
{
    public class AccessUser: IdentityUser
    { 
        public string IsAdmin { get; set; } 
    }
}
