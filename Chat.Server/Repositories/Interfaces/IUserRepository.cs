using Chat.Server.Entities;
using Chat.Server.Enums;

namespace Chat.Server.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Status Check(User user);
        bool SignIn(User user);
        bool SignUp(User user);
        bool SignOut(User user);
        bool Ban(User user);
        bool Unban(User user);
    }
}
