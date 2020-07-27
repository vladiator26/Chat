using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer
{
    public enum Status
    {
        None,
        Ok,
        WrongUsername,
        WrongPassword,
        Banned,
        AlreadyLogined
    }
}
