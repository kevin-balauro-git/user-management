using System.ComponentModel.DataAnnotations;

namespace UserManagement.API.Dto
{
    public class NameDto
    {
        [MinLength(4)]
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only alphabets")]
        public string FirstName { get; set; }

        [MinLength(4)]
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only alphabets")]
        public string LastName { get; set; }
    }
}
