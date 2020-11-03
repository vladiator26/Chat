using Chat.Server.Enums;

namespace Chat.Server.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; } = Role.User;
        public bool IsBanned { get; set; } = false;
        public bool IsSignedIn { get; set; } = false;
    }
}
