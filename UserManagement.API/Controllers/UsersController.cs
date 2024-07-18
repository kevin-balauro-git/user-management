using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UserManagement.API.Dto;
using UserManagement.API.Entities;
using UserManagement.API.Interface;

namespace UserManagement.API.Controllers
{

    [ApiController]
    [Route("api/users")]
        public class UsersController: ControllerBase
    {
        
        private readonly ILogger<UsersController> _logger;
        
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UsersController(
            IUserRepository userRepository,
            ILogger<UsersController> logger,
            IMapper mapper,         
            UserManager<User> userManager,
            RoleManager<Role> roleManager) 
        {
            _logger = logger;
            _mapper = mapper;
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        [HttpGet]
 //       [Authorize]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers(
            [FromQuery] PaginationDto paginationDto,
            [FromQuery] string searchItem = "", 
            [FromQuery] string sortOrder= "desc"       
            )
        {
            _logger.LogInformation("Pagination info: {@pagination}", paginationDto);
            UserRoleDto userRoleDto = new UserRoleDto {
                RoleName = User.FindFirst(ClaimTypes.Role)?.Value,
                UserName = User.FindFirst(ClaimTypes.GivenName)?.Value
            };
            if(userRoleDto.RoleName.Equals(null) || userRoleDto.UserName.Equals(null))
                return Unauthorized();
           
            var usersDto = await _userRepository.GetUsersAsync( searchItem, sortOrder, userRoleDto, paginationDto);
            _logger.LogInformation("Count: {@count}", paginationDto.totalCount);
            var result = new
            {
                pagination = paginationDto,
                users = usersDto
            };
            return Ok(result);     
        }
        
        [HttpGet]
//        [Authorize]
        [Route("{id:int}")]
        public async Task<ActionResult<UserDto>> GetUser([FromRoute] int id)
        {
            var userDto = await _userRepository.GetUserAsync(id);
            
            if(userDto == null)
                return NotFound();

            return Ok(userDto);
        }
        
        [HttpPost]
//        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> CreateUser([FromBody] UserDto newUserDto)
        {
            _logger.LogInformation("New User: {@user}", newUserDto);
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("MOdel state: {@model}", ModelState);
                return BadRequest(ModelState);
            }
                
            if (await _userRepository.checkEmail(newUserDto))
                return Conflict($"An existing record with an email '{newUserDto.Email}' was already found.");

            if (await _userRepository.checkUsername(newUserDto))
                return Conflict($"An existing record with a username '{newUserDto.UserName}' was already found.");
 
            var newUser = _userRepository.CreateUserAsync(newUserDto).Result;
           
            return CreatedAtAction(nameof(GetUser), new {id = newUser.Id}, newUser );
        }
        
        [HttpPut]
//        [Authorize(Policy = "Admin")]
        [Route("{id:int}")]
        public async Task<ActionResult> UpdateUser([FromRoute] int id, [FromBody] UserDto updateUserDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState); 
            if (await _userRepository.checkEmail(updateUserDto) && await _userRepository.checkId(updateUserDto))
                return Conflict($"An existing record with an email '{updateUserDto.Email}' was already found.");  
            if (await _userRepository.checkUsername(updateUserDto) && await _userRepository.checkId(updateUserDto))
                return Conflict($"An existing record with a username '{updateUserDto.UserName}' was already found.");
            var updateUser = await _userRepository.UpdateUserAsync(id, updateUserDto);            
            if(updateUser == null)
                return NotFound();
            return NoContent();
        }

        [HttpDelete]
//        [Authorize(Policy = "Admin")]
        [Route("{id:int}")]
        public async Task<ActionResult<User>> DeleteUser([FromRoute] int id)
        {
            var user = await _userRepository.DeleteUserAsync(id);
            
            if(user == null)
                return BadRequest();
            
            return NoContent();
        }
    }
}
