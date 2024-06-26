using System.ComponentModel.DataAnnotations;

namespace UserManagement.API.Dto
{
    public class LoginUserDto
    {
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}
