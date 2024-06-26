using UserManagement.API.Entities;

namespace UserManagement.API.Interface
{
    public interface IJwtService
    {
       string GenerateJwt(AccessUser user);
    }
}
