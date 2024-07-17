

using System.ComponentModel.DataAnnotations;

namespace UserManagement.API.Dto
{
    public class UserDto
{
    public int Id { get; set; }
    public NameDto Name { get; set; }
    [MinLength(6)]
    [Required]
    public string UserName { get; set; }

    [EmailAddress]
    [Required]
    public string Email { get; set; }
    
    [MinLength(8)]
    [Required]
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public AddressDto Address { get; set; } 
 

}
}
