using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ChatServer
{
    public class User
    {
        public int Id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public Role role { get; set; } = Role.User;
        public bool banned { get; set; } = false;
        public bool signed { get; set; } = false;
    }
}
