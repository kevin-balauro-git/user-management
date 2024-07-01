using Microsoft.AspNetCore.Identity;

namespace UserManagement.API.Entities
{
    public class User
    {
        public Guid Uuid { get; private set; }
        public int Id { get; }
        public Name Name { get; set; } = new Name();
        public string Username  { get; set; } = string.Empty;
        public string Email { get; set; } =string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public Address Address { get; set; } = new Address();
        public string IsAdmin { get; set; }
        public User()
        {
            if (Uuid == Guid.Empty)
                Uuid = Guid.NewGuid();
        }

    }
}
