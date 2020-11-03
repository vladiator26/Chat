using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Chat.Client
{
    public partial class LoginForm : Form
    {
        public Socket connection;
        public Form callback;
        public bool result;
        public ChatForm form;
        public Thread checkThread;

        public LoginForm(Socket socket, Form form)
        {
            InitializeComponent();
            connection = socket;
            callback = form;
            checkThread = new Thread(CheckConnected);
            checkThread.Start();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            connection.Send(Encoding.UTF8.GetBytes("login|" + UsernameTextBox.Text + "|" + PasswordTextBox.Text));
            byte[] buffer = new byte[8196];
            int length = connection.Receive(buffer);
            string answer = Encoding.UTF8.GetString(buffer, 0, length);
            if (answer == "ok")
            {
                Hide();
                form = new ChatForm(connection, this, UsernameTextBox.Text, result);
                form.Show();
            }
            else
            {
                switch (answer)
                {
                    case "al":
                        MessageBox.Show("Already logined!");
                        break;
                    case "wl":
                        MessageBox.Show("Wrong login!");
                        break;
                    case "wp":
                        MessageBox.Show("Wrong password!");
                        break;
                    case "ub":
                        MessageBox.Show("Banned!");
                        break;
                    default:
                        MessageBox.Show("Unknown error!");
                        break;
                }
                return;
            }
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            checkThread.Abort();
            checkThread.Join();
            callback.Show();
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            connection.Send(Encoding.UTF8.GetBytes("register|" + UsernameTextBox.Text + "|" + PasswordTextBox.Text));
            byte[] buffer = new byte[8196];
            int length = connection.Receive(buffer);
            string answer = Encoding.UTF8.GetString(buffer, 0, length);
            if (answer == "ok")
            {
                Hide();
                form = new ChatForm(connection, this, UsernameTextBox.Text, result);
                form.Show();
            }
            else
            {
                MessageBox.Show(answer);
                return;
            }
        }

        public void CheckConnected()
        {
            while (true)
            {
                if (connection.Connected == false)
                {
                    Invoke((MethodInvoker)(() => form.Close()));
                    Invoke((MethodInvoker)(() => Close()));
                    return;
                }
            }
        }
    }
}