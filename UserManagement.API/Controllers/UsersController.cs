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
        private readonly UserContext _userContext;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AccessUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(
            IUserRepository userRepository,
            ILogger<UsersController> logger,
            IMapper mapper,
            UserContext userContext,
            UserManager<AccessUser> userManager,
            RoleManager<IdentityRole> roleManager) 
        {
            _logger = logger;
            _userContext = userContext;
            _mapper = mapper;
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery] string searchItem = "", [FromBody] string sortOrder="desc")
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized("You're not authenticated");
            

            var users = await _userContext.Users.Where(u =>
                 u.Name.FirstName.ToLower().Contains(searchItem.ToLower()) ||
                 u.Name.LastName.ToLower().Contains(searchItem.ToLower()) ||
                 u.Email.ToLower().Contains(searchItem.ToLower()) ||
                 u.Username.ToLower().Contains(searchItem.ToLower()) ||
                 u.Phone.ToLower().Contains(searchItem.ToLower())
            ).ToListAsync();

            await _userContext.Users.ToListAsync(); 
            switch(sortOrder)
            {
                case "desc":
                     users.OrderByDescending(u => u.Id);
                    break;
                default:
                    users.OrderBy(u => u.Id);
                    break;
            }

            var usersDto = users.Select(u => _mapper.Map<UserDto>(u)).ToList();
   
            return Ok(usersDto);
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
            _logger.LogInformation("Authenticated: {@auth}", User.Identity.IsAuthenticated);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _userRepository.checkEmail(newUserDto))
                return Conflict($"An existing record with an email '{newUserDto.Email}' was already found.");

            if (await _userRepository.checkUsername(newUserDto))
                return Conflict($"An existing record with a username '{newUserDto.Username}' was already found.");
 
            var newUser = _userRepository.CreateUserAsync(newUserDto).Result;
           
            return CreatedAtAction(nameof(GetUser), new {id = newUser.Id}, newUser );
        }

        [HttpPut]
        [Authorize(Policy = "Admin")]
        [Route("{id:int}")]
        public async Task<ActionResult> UpdateUser([FromRoute] int id, [FromBody] UserDto updateUserDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            _logger.LogInformation("Authenticated: {@auth}", User.Identity.IsAuthenticated);
            if (await _userRepository.checkEmail(updateUserDto) && await _userRepository.checkId(updateUserDto))
                return Conflict($"An existing record with an email '{updateUserDto.Email}' was already found.");
            
            if (await _userRepository.checkUsername(updateUserDto) && await _userRepository.checkId(updateUserDto))
            return Conflict($"An existing record with a username '{updateUserDto.Username}' was already found.");
            
            var updateUser = await _userRepository.UpdateUserAsync(id, updateUserDto);
            
            if(updateUser == null)
                return NotFound();
            
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
            
            return NoContent();
        }


    }
}
