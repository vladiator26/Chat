using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            new Server().Start();
        }
    }
}
