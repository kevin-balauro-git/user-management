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
        private readonly UserContext _userContext;
        private readonly IMapper _mapper;
        private readonly UserManager<AccessUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRepository(
            ILogger<UsersController> logger,
            UserContext userContext,
            UserManager<AccessUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            _logger = logger;
            _userContext = userContext;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<UserDto>> GetUsersAsync(string searchItem, string sortOrder)
        {
            var users = await _userContext.Users.Where(u =>
                 u.Name.FirstName.ToLower().Contains(searchItem.ToLower()) ||
                 u.Name.LastName.ToLower().Contains(searchItem.ToLower()) ||
                 u.Email.ToLower().Contains(searchItem.ToLower()) ||
                 u.Username.ToLower().Contains(searchItem.ToLower()) ||
                 u.Phone.ToLower().Contains(searchItem.ToLower())
            ).ToListAsync();

            
            var usersDto = users.Select(u => _mapper.Map<UserDto>(u)).ToList();

            return usersDto;
        }


        public async Task<UserDto> GetUserAsync(int id)
        {
           var user =  await _userContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
                return _mapper.Map<UserDto>(user);
            return null;
        }

        public async Task<User> CreateUserAsync(UserDto newUserDto)
        {
            User? newUser = _mapper.Map<User>(newUserDto);
            
            var newAccessUser = new AccessUser
            {    
                Id = newUser.Uuid.ToString(),
                UserName = newUserDto.Username,
                Email = newUserDto.Email,
                IsAdmin = newUserDto.IsAdmin,
            };
             
            await _userManager.CreateAsync(newAccessUser, newUserDto.Password);
            
            var adminRoleExist = await _roleManager.RoleExistsAsync("Admin");
            var employeeRoleExist = await _roleManager.RoleExistsAsync("Employee");

            if (!adminRoleExist && !employeeRoleExist)
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("Employee"));
            }

            if (bool.Parse(newUserDto.IsAdmin))
                await _userManager.AddToRoleAsync(newAccessUser, "Admin ");
            else
                await _userManager.AddToRoleAsync(newAccessUser, "Employee");
  
            _userContext.Add(newUser);
            await _userContext.SaveChangesAsync();
            
                      
            return newUser;
        }

        public async Task<User> UpdateUserAsync(int id,UserDto updatedUserDto)
        {
            var updatedUser = await _userContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            var users = await _userManager.Users.ToListAsync();
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == updatedUser.Uuid.ToString());
            
            if (updatedUser != null)
            {
                updatedUser.Name.FirstName = updatedUserDto.Name.FirstName;
                updatedUser.Name.LastName = updatedUserDto.Name.LastName;
                updatedUser.Email = updatedUserDto.Email;
                updatedUser.Username = updatedUserDto.Username;
                updatedUser.Password = updatedUserDto.Password;
                updatedUser.Phone = updatedUserDto.Phone;
                updatedUser.IsAdmin = updatedUserDto.IsAdmin;
                updatedUser.Address.StreetNumber = updatedUserDto.Address.StreetNumber;
                updatedUser.Address.StreetName = updatedUserDto.Address.StreetName;
                updatedUser.Address.City = updatedUserDto.Address.City;
                updatedUser.Address.ZipCode = updatedUserDto.Address.ZipCode;
                updatedUser.Address.GeoLocation.Latitude = updatedUserDto.Address.GeoLocation.Latitude;
                updatedUser.Address.GeoLocation.Longitude = updatedUserDto.Address.GeoLocation.Longitude;

                user.IsAdmin = updatedUser.IsAdmin;

                await _userContext.SaveChangesAsync();
                await _userManager.UpdateAsync(user);
                return updatedUser;
            }
            return null;
        }

        public async Task<User> DeleteUserAsync(int id)
        {
            _logger.LogInformation("User Id: {@id}", id);
            var user = await _userContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            var accessUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == user.Uuid.ToString());
            if (user != null)
            {
                _userContext.Users.Remove(user);
                await _userContext.SaveChangesAsync();
                await _userManager.DeleteAsync(accessUser);
                return user;
            }
            return null;
            
        }

        public async Task<Boolean> checkEmail(UserDto userDto)
        {
           var user = await _userContext.Users.FirstOrDefaultAsync(u => u.Email.Equals(userDto.Email));
            if(user != null)
                return true;
            
            return false;
        }

        public async Task<Boolean> checkUsername(UserDto userDto)
        {
            var user = await _userContext.Users.FirstOrDefaultAsync(u => u.Username.Equals(userDto.Username));
            if (user != null)
                return true;

            return false;
        }

        public async Task<Boolean> checkId(UserDto userDto)
        {
            var user = await _userContext.Users.FirstOrDefaultAsync(u => u.Id.Equals(userDto.Id));
            if(user != null) return true;
            return false;
        }
    }
}
