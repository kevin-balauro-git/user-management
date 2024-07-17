using UserManagement.API.Entities;

namespace UserManagement.API.Interface
{
    public interface IJwtService
    {
        Task<string> GenerateJwt(User user);
    }
}
