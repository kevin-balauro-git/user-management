using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagement.API.Controllers;
using UserManagement.API.Dto;
using UserManagement.API.Entities;
using UserManagement.API.Interface;

namespace UserManagement.API.Repository
{
    public class UserRepository : IUserRepository
    {

        private readonly ILogger<UsersController> _logger;

        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public UserRepository(
            ILogger<UsersController> logger,
            IMapper mapper,
            UserManager<User> userManager,
            RoleManager<Role> roleManager
            )
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<UserDto>> GetUsersAsync(
            string searchItem,
            string sortOrder,
            string role,
            string username)
        {
            IList<User> users = new List<User>();

            if (role.Equals("Employee"))
            {
                users = await _userManager.GetUsersInRoleAsync(role);
            }

            if (role.Equals("Admin"))
            {
                users = await _userManager.Users.ToListAsync();
            }

            var filteredUsers = users
                .Where(u => !u.UserName!.Contains(username))
                .Where(u =>
                 u.Name.FirstName.ToLower().Contains(searchItem.ToLower()) ||
                 u.Name.LastName.ToLower().Contains(searchItem.ToLower()) ||
                 u.Email!.ToLower().Contains(searchItem.ToLower()) ||
                 u.PhoneNumber!.ToLower().Contains(searchItem.ToLower()) ||
                 u.UserName!.ToLower().Contains(searchItem.ToLower()))
                .ToList();

            var usersDto = filteredUsers.Select(u => _mapper.Map<UserDto>(u)).ToList();
            if (sortOrder.Equals("desc"))
                return usersDto.OrderBy(u => u.Id).ToList();
            return usersDto.OrderByDescending(u => u.Id).ToList();
        }


        public async Task<UserDto> GetUserAsync(int id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
                return _mapper.Map<UserDto>(user);
            return null;
        }

        public async Task<User> CreateUserAsync(UserDto newUserDto)
        {
            User newUser = _mapper.Map<User>(newUserDto);
            newUser.SecurityStamp = Guid.NewGuid().ToString();
            try
            {
                await _userManager.CreateAsync(newUser, newUser.Password);

                await _userManager.AddToRoleAsync(newUser, "Employee");
            }
            catch (AggregateException ex)
            {
                _logger.LogInformation("Aggregate Error: {@message}", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error: {@message}", ex.Message);
            }


            return newUser;
        }

        public async Task<User> UpdateUserAsync(int id, UserDto updatedUserDto)
        {
            var updatedUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (updatedUser != null)
            {
                updatedUser.Name.FirstName = updatedUserDto.Name.FirstName;
                updatedUser.Name.LastName = updatedUserDto.Name.LastName;
                updatedUser.Email = updatedUserDto.Email;
                updatedUser.UserName = updatedUserDto.UserName;
                updatedUser.Password = updatedUserDto.Password;
                updatedUser.PhoneNumber = updatedUserDto.PhoneNumber;
                updatedUser.Address.StreetNumber = updatedUserDto.Address.StreetNumber;
                updatedUser.Address.StreetName = updatedUserDto.Address.StreetName;
                updatedUser.Address.City = updatedUserDto.Address.City;
                updatedUser.Address.ZipCode = updatedUserDto.Address.ZipCode;
                updatedUser.Address.GeoLocation.Latitude = updatedUserDto.Address.GeoLocation.Latitude;
                updatedUser.Address.GeoLocation.Longitude = updatedUserDto.Address.GeoLocation.Longitude;
                updatedUser.DateUpdated = DateTime.UtcNow;
                await _userManager.UpdateAsync(updatedUser);
                return updatedUser;
            }
            return null;
        }

        public async Task<User> DeleteUserAsync(int id)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
                _logger.LogInformation("Deleted User: {@user}", user);
                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                    return user;
                }
            }

            catch (ArgumentNullException ex)
            {
                _logger.LogInformation("Error: User not found");
            }

            return null;
        }

        public async Task<Boolean> checkEmail(UserDto userDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email.Equals(userDto.Email));
            if (user != null)
                return true;

            return false;
        }

        public async Task<Boolean> checkUsername(UserDto userDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.Equals(userDto.UserName));
            if (user != null)
                return true;

            return false;
        }

        public async Task<Boolean> checkId(UserDto userDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id.Equals(userDto.Id));
            if (user != null) return true;
            return false;
        }
    }

}
