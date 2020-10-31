using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Chat.Client
{
    public partial class ChatForm : Form
    {
        bool closed = false;
        public bool back=false;
        Form callback;
        Socket socket;
        string username;
        Thread thread;
        public ChatForm(Socket connection,Form form,string user,bool result)
        {
            InitializeComponent();
            socket = connection;
            callback = form;
            username = user;
            back = result;
            thread=new Thread(MessageUpdater);
            thread.Start();
        }

        private void sendMessageButton_Click(object sender, EventArgs e)
        {
            socket.Send(Encoding.UTF8.GetBytes(messageTextBox.Text));
            messageBox.Text = username + ":" + messageTextBox.Text + Environment.NewLine + messageBox.Text;
            messageTextBox.Text = "";
        }

        private void MessageUpdater()
        {
            while (true)
            {
                try
                {
                    if (closed)
                    {
                        return;
                    }
                    byte[] buffer = new byte[8196];
                    socket.ReceiveTimeout = 1000;
                    int length = socket.Receive(buffer);
                    string[] message = Encoding.UTF8.GetString(buffer, 0, length).Split(':');
                    string text = messageBox.Text;
                    messageBox.Invoke((MethodInvoker)(() => messageBox.Text = message[0] + ":" + message[1].Split('\0')[0] + Environment.NewLine + text));
                    Thread.Sleep(1000);
                }
                catch (SocketException ex)
                {
                    if (ex.ErrorCode != 10060)
                    {
                        MessageBox.Show("Соеденение потеряно");
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                        back = true;
                        break;
                    }
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                socket.Send(Encoding.UTF8.GetBytes("logout"));
            }
            catch { }
            thread.Abort();
            thread.Join();
            closed = true;
            callback.Show();
        }

        private void messageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                sendMessageButton_Click(sender,e);
            }
        }
    }
}
