using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Chat.Server
{
    public class Server
    {
        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
        static ConsoleEventDelegate handler;
        public List<Socket> ConnectedUsers = new List<Socket>();
        public Thread thread;
        public Socket socket;
        public Socket client;
        public IPEndPoint endPoint;
        public static DbHandler dbHandler = new DbHandler();

        public bool Start()
        {
            Console.WriteLine("Starting database handler");
            dbHandler.Start();
            Console.WriteLine("Database handler successfully started!");
            handler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(handler, true);
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress address = ipHostInfo.AddressList[5];
            IPEndPoint endPoint = new IPEndPoint(address, 19132);
            socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.ExclusiveAddressUse = true;
            socket.Bind(endPoint);
            socket.Listen(1000);
            while (true)
            {
                client = socket.Accept();
                thread = new Thread(ClientHandle);
                thread.IsBackground = true;
                thread.Start();
            }
        }

        public void ClientHandle()
        {
            Console.WriteLine("User {0} connected", client.RemoteEndPoint);
            Socket socket = client;
            byte[] buffer;
            User cur=null;
            Status status;
            bool ok = false;
            try
            {
                while (true)
                {
                    status = Status.None;
                    ok = false;
                    while (status != Status.Ok && ok == false)
                    {
                        buffer = new byte[8196];
                        int length = socket.Receive(buffer, SocketFlags.None);
                        string answer = Encoding.UTF8.GetString(buffer, 0, length);
                        string[] auth = answer.Split('|');
                        if (auth.Length > 1)
                        {
                            cur = new User() {username = auth[1], password = auth[2].Split('\0')[0]};
                        }
                        else
                        {
                            continue;
                        }
                        if (auth[0] == "login")
                        {
                            status = dbHandler.CheckUser(cur);
                            switch (status)
                            {
                                case Status.WrongUsername:
                                    socket.Send(Encoding.UTF8.GetBytes("wl"));
                                    break;
                                case Status.WrongPassword:
                                    socket.Send(Encoding.UTF8.GetBytes("wp"));
                                    break;
                                case Status.Banned:
                                    socket.Send(Encoding.UTF8.GetBytes("ub"));
                                    break;
                                case Status.Ok:
                                    socket.Send(Encoding.UTF8.GetBytes("ok"));
                                    break;
                                case Status.AlreadyLogined:
                                    socket.Send(Encoding.UTF8.GetBytes("al"));
                                    break;
                            }
                        }
                        else if (auth[0] == "register")
                        {
                            ok = dbHandler.AddUser(cur);
                            if (ok)
                            {
                                socket.Send(Encoding.UTF8.GetBytes("ok"));
                            }
                            else
                            {
                                socket.Send(Encoding.UTF8.GetBytes("ae"));
                            }
                        }
                    }
                    ConnectedUsers.Add(socket);
                    Console.WriteLine("{0} connected as {1}",socket.RemoteEndPoint,cur.username);
                    dbHandler.Login(cur);
                    cur.signed = true;
                    while (true)
                    {
                        buffer = new byte[8196];
                        int length=socket.Receive(buffer, SocketFlags.None);
                        string answer = Encoding.UTF8.GetString(buffer, 0, length);
                        if (answer=="logout")
                        {
                            dbHandler.Logout(cur);
                            ConnectedUsers.Remove(socket);
                            break;
                        }
                        byte[] message = Encoding.UTF8.GetBytes(cur.username + ":" + answer);
                        Console.WriteLine("New message from " + socket.RemoteEndPoint);
                        foreach (Socket user in ConnectedUsers)
                        {
                            if (user != socket)
                            {
                                user.Send(message);
                            }
                        }
                    }
                }
            }
            catch
            {
                dbHandler.Logout(cur);
                ConnectedUsers.Remove(socket);
            }
        }

        static bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                dbHandler.Stop();
            }
            return false;
        }
    }
}
