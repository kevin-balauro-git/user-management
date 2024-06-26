using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagement.API.Dto;
using UserManagement.API.Entities;
using UserManagement.API.Interface;
using UserManagement.API.Services;

namespace UserManagement.API.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController: ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<AccessUser> _userManager;
        private readonly SignInManager<AccessUser> _signInManager;
        private readonly IJwtService _jwtService;

        public AccountController(
            ILogger<AccountController> logger,
            SignInManager<AccessUser> signInManager,
            UserManager<AccessUser> userManager, 
            IJwtService jwtService)
        {
            _logger = logger;
            _userManager = userManager;
           _jwtService = jwtService;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginUserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var accessUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email.Equals(userDto.Email));

            if (accessUser == null)
                return Unauthorized("Invalid Username");

            var signInResult = await _signInManager.CheckPasswordSignInAsync(accessUser, userDto.Password, false);
            
            if (!signInResult.Succeeded)
                return Unauthorized("Invalid Password");
                        
            var token = _jwtService.GenerateJwt(accessUser);

            return Ok(new AccessUserDto
            {
                UserName = accessUser.UserName,
                Email = accessUser.Email,
                IsAdmin = accessUser.IsAdmin,
                Token = token
            });
        }

        
    }
}
