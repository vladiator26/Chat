using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public class DbHandler
    {
        public Status CheckUser(User user)
        {
            using (AppContext db = new AppContext())
            {
                foreach (User cur in db.Users.ToList())
                {
                    if (cur.username == user.username)
                    {
                        if (cur.password == user.password)
                        {
                            if (cur.banned)
                            {
                                return Status.Banned;
                            }
                            if (cur.signed)
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
        }

        public bool AddUser(User user)
        {
            using (AppContext db = new AppContext())
            {
                foreach (User cur in db.Users.ToList())
                {
                    if (cur.username==user.username)
                    {
                        return false;
                    }
                }
                db.Users.Add(user);
                db.SaveChanges();
                return true;
            }
        }

        public bool DeleteUser(User user)
        {
            using (AppContext db = new AppContext())
            {
                foreach (User cur in db.Users.ToList())
                {
                    if (cur.username == user.username)
                    {
                        return false;
                    }
                }
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
        }

        public bool BanUser(User user)
        {
            using (AppContext db = new AppContext())
            {
                foreach (User cur in db.Users.ToList())
                {
                    if (cur.username == user.username)
                    {
                        if (cur.banned)
                        {
                            return false;
                        }
                        cur.banned = true;
                        user.banned = true;
                    }
                }
                db.SaveChanges();
                return true;
            }
        }

        public bool UnbanUser(User user)
        {
            using (AppContext db = new AppContext())
            {
                foreach (User cur in db.Users.ToList())
                {
                    if (cur.username == user.username)
                    {
                        if (cur.banned)
                        {
                            cur.banned = false;
                            user.banned = false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                db.SaveChanges();
                return true;
            }
        }

        public bool Login(User user)
        {
            using (AppContext db = new AppContext())
            {
                foreach (User cur in db.Users.ToList())
                {
                    if (cur.username == user.username)
                    {
                        if (cur.signed)
                        {
                            return false;
                        }
                        user.signed = true;
                        cur.signed = true;
                    }
                }

                db.SaveChanges();
                return true;
            }
        }

        public bool Logout(User user)
        {
            using (AppContext db = new AppContext())
            {
                foreach (User cur in db.Users.ToList())
                {
                    if (cur.username == user.username)
                    {
                        if (!cur.signed)
                        {
                            return false;
                        }
                        cur.signed = false;
                        user.signed = false;
                    }
                }
                db.SaveChanges();
                return true;
            }
        }

        public bool Start()
        {
            using (AppContext db = new AppContext())
            {
                db.SaveChanges();
                return true;
            }
        }

        public bool Stop()
        {
            using (AppContext db = new AppContext())
            {
                foreach (User cur in db.Users.ToList())
                {
                    cur.signed = false;
                }
                db.SaveChanges();
                return true;
            }
        }
    }
}
