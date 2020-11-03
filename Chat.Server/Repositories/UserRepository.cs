using System.Linq;
using Chat.Server.Entities;
using Chat.Server.Enums;
using Chat.Server.Repositories.Base;
using Chat.Server.Repositories.Interfaces;

namespace Chat.Server.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext context) : base(context)
        {
        }

        public Status Check(User user)
        {
            foreach (User cur in _dbSet.ToList())
            {
                if (cur.Username == user.Username)
                {
                    if (cur.Password == user.Password)
                    {
                        if (cur.IsBanned)
                        {
                            return Status.Banned;
                        }
                        if (cur.IsSignedIn)
                        {
                            return Status.AlreadyLogined;
                        }
                        return Status.Ok;
                    }
                    else
                    {
                        return Status.WrongPassword;
                    }
                }
            }
            return Status.WrongUsername;
        }

        public bool SignIn(User user)
        {
            foreach (User cur in _dbSet.ToList())
            {
                if (cur.Username == user.Username)
                {
                    if (cur.IsSignedIn)
                    {
                        return false;
                    }
                    user.IsSignedIn = true;
                    cur.IsSignedIn = true;
                }
            }

            SaveChanges();
            return true;
        }

        public bool SignUp(User user)
        {
            throw new System.NotImplementedException();
        }

        public bool SignOut(User user)
        {
            foreach (User cur in _dbSet.ToList())
            {
                if (cur.Username == user.Username)
                {
                    if (!cur.IsSignedIn)
                    {
                        return false;
                    }
                    cur.IsSignedIn = false;
                    user.IsSignedIn = false;
                }
            }
            SaveChanges();
            return true;
        }

        public bool Ban(User user)
        {
            foreach (User cur in _dbSet.ToList())
            {
                if (cur.Username == user.Username)
                {
                    if (cur.IsBanned)
                    {
                        return false;
                    }
                    cur.IsBanned = true;
                    user.IsBanned = true;
                }
            }
            SaveChanges();
            return true;
        }

        public bool Unban(User user)
        {
            foreach (User cur in _dbSet.ToList())
            {
                if (cur.Username == user.Username)
                {
                    if (cur.IsBanned)
                    {
                        cur.IsBanned = false;
                        user.IsBanned = false;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            SaveChanges();
            return true;
        }
    }
}