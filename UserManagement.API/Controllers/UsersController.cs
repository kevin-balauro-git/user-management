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
        private readonly UserContext _userContext;

        public UsersController(
            IUserRepository userRepository,
            ILogger<UsersController> logger,
            IMapper mapper,         
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            UserContext userContext) 
        {
            _logger = logger;
            _mapper = mapper;
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _userContext = userContext;
        }
        
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers(
            [FromQuery] PaginationDto paginationDto,
            [FromQuery] string searchItem = "", 
            [FromQuery] string sortOrder= "desc"       
            )
        {
          
            UserRoleDto userRoleDto = new UserRoleDto {
                RoleName = User.FindFirst(ClaimTypes.Role)?.Value!,
                UserName = User.FindFirst(ClaimTypes.GivenName)?.Value!
            };

            if(userRoleDto.RoleName!.Equals(null) || userRoleDto.UserName!.Equals(null))
                return Unauthorized();
           
            var usersDto = await _userRepository.GetUsersAsync( searchItem, sortOrder, userRoleDto, paginationDto);
           
            var result = new
            {
                pagination = paginationDto,
                users = usersDto
            };

            return Ok(result);     
        }
        
        [HttpGet]
        [Authorize]
        [Route("{id:int}")]
        public async Task<ActionResult<UserDto>> GetUser([FromRoute] int id)
        {
            var userDto = await _userRepository.GetUserAsync(id);
            
            if(userDto == null)
                return NotFound();

            return Ok(userDto);
        }
        
        [HttpPost]
       [Authorize(Policy = "Admin")]
        public async Task<ActionResult> CreateUser([FromBody] UserDto newUserDto)
        {           
            if (!ModelState.IsValid)
                return BadRequest(ModelState);         
            if (await _userRepository.checkEmail(newUserDto))
                return Conflict($"An existing record with an email '{newUserDto.Email}' was already found.");
            if (await _userRepository.checkUsername(newUserDto))
                return Conflict($"An existing record with a username '{newUserDto.UserName}' was already found.");
 
            var newUser = _userRepository.CreateUserAsync(newUserDto).Result;
            
            await _userContext.Histories.AddAsync(new History() { UserId=Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value), HttpVerb ="CREATE" });
            await _userContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new {id = newUser.Id}, newUser );
        }
        
        [HttpPut]
        [Authorize]
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

            await _userContext.Histories.AddAsync(new History() { UserId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value), HttpVerb = "UPDATE" });
            await _userContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [Authorize(Policy = "Admin")]
        [Route("{id:int}")]
        public async Task<ActionResult<User>> DeleteUser([FromRoute] int id)
        {
            var user = await _userRepository.DeleteUserAsync(id);
            
            if(user == null)
                return BadRequest();
            
            await _userContext.Histories.AddAsync(new History() { UserId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value), HttpVerb = "DELETE" });
            await _userContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
