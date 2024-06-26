using AutoMapper;
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

        public UserRepository(
            ILogger<UsersController> logger,
            UserContext userContext,
            IMapper mapper)
        {
            _logger = logger;
            _userContext = userContext;
            _mapper = mapper;
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
            _userContext.Add(newUser);
            await _userContext.SaveChangesAsync();
            return newUser;
        }

        public async Task<User> UpdateUserAsync(int id,UserDto updatedUserDto)
        {
            var updatedUser = await _userContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (updatedUser != null)
            {
                updatedUser.Name.FirstName = updatedUserDto.Name.FirstName;
                updatedUser.Name.LastName = updatedUserDto.Name.LastName;
                updatedUser.Email = updatedUserDto.Email;
                updatedUser.Username = updatedUserDto.Username;
                updatedUser.Password = updatedUserDto.Password;
                updatedUser.Phone = updatedUserDto.Phone;
                updatedUser.Address.StreetNumber = updatedUserDto.Address.StreetNumber;
                updatedUser.Address.StreetName = updatedUserDto.Address.StreetName;
                updatedUser.Address.City = updatedUserDto.Address.City;
                updatedUser.Address.ZipCode = updatedUserDto.Address.ZipCode;
                updatedUser.Address.GeoLocation.Latitude = updatedUserDto.Address.GeoLocation.Latitude;
                updatedUser.Address.GeoLocation.Longitude = updatedUserDto.Address.GeoLocation.Longitude;

                _logger.LogInformation("Updated User: {@user}", updatedUser);

                await _userContext.SaveChangesAsync();
                return updatedUser;
            }
            return null;
        }

        public async Task<User> DeleteUserAsync(int id)
        {
            _logger.LogInformation("User Id: {@id}", id);
            var user = await _userContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                _logger.LogInformation("User Deleted: {@user}", user);

                _userContext.Users.Remove(user);
                await _userContext.SaveChangesAsync();

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
