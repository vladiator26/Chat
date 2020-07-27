using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class MainForm : Form
    {
        public Socket socket;
        public MainForm()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress address = IPAddress.Parse(IPTextBox.Text);
                IPEndPoint endPoint = new IPEndPoint(address, Convert.ToInt32(PortTextBox.Text));
                socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(endPoint);
                Hide();
                new LoginForm(socket, this).Show();
            }
            catch
            {
                MessageBox.Show("Неправильный сервер");
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {

        }
    }
}
