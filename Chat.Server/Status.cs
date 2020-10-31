using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Server
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
