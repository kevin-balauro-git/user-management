using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using UserManagement.API.Dto;
using UserManagement.API.Entities;

namespace UserManagement.API.Interface
{
    public interface IUserRepository
    {
        Task<List<UserDto>> GetUsersAsync(string searchItem, string sortOrder, string role, string username);
        Task<UserDto> GetUserAsync(int id);
        Task<User> CreateUserAsync(UserDto newUserDto);
        Task<User> UpdateUserAsync(int id, UserDto updatedUserDto);
        Task<User> DeleteUserAsync(int id);
        Task<bool> checkEmail(UserDto userDto);
        Task<bool> checkUsername(UserDto userDto);
        Task<bool> checkId(UserDto userDto);

    }
}
