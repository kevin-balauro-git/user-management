

using System.ComponentModel.DataAnnotations;

namespace UserManagement.API.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public NameDto Name { get; set; } = new NameDto();

        [MinLength(6)]
        [Required]
        public string Username { get; set; } = string.Empty;

        [EmailAddress]
        [Required]
        public string Email { get; set; } = string.Empty;
        
        [MinLength(8)]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public AddressDto Address { get; set; } = new AddressDto();
        public string IsAdmin { get; set; }
    }
}
