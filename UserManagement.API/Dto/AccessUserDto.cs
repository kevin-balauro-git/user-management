namespace UserManagement.API.Dto
{
    public class AccessUserDto
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public string IsAdmin { get; set; }
    }
}
