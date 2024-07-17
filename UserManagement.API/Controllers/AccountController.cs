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
    [Route("api/account")]
    public class AccountController : ControllerBase
    {

        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtService _jwtService;

        public AccountController(
            ILogger<AccountController> logger,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IJwtService jwtService)
        {
            _logger = logger;
            _userManager = userManager;
            _jwtService = jwtService;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email.Equals(userDto.Email) || u.UserName.Equals(userDto.Email));

            if (user == null)
                return Unauthorized("Invalid Username");

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, userDto.Password, false);

            if (!signInResult.Succeeded)
                return Unauthorized("Invalid Password");

            var token = _jwtService.GenerateJwt(user);

            return Ok(new ClaimUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Token = await token
            });
        }

        [HttpGet]
        [Route("current-user")]
        public async Task<IActionResult> CurrentUser()
        {
            var username = User.FindFirst(ClaimTypes.GivenName)?.Value;
            if (username == null)
                return BadRequest();

            var user = await _userManager.Users
                .Where(u => u.UserName.Equals(username))
                .Select(u => new
                {
                    Id = u.Id,
                    Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                }).FirstOrDefaultAsync();

            return Ok(user);
        }

    }

}
