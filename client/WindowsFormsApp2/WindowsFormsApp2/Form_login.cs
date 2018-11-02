using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form_login : Form
    {
        private string serverIp = "144.34.132.4";
        private int serverPort = 5188;
        private Socket mySocket;
        private string login_account = "";
        private Form_main myParent;

        public Form_login()
        {
            InitializeComponent();

            mySocket = connectServer(serverIp, serverPort);

            if(mySocket.Connected) {
                labelstate.Text = "服务器连接正常";
                labelstate.ForeColor = Color.Green;
            } else {
                labelstate.Text = "服务器连接失败";
                labelstate.ForeColor = Color.Red;
            }
        }

        public void setParent(Form_main parent)
        {
            myParent = parent;
        }

        public string GetLoginAccount()
        {
            return login_account;
        }

        public Socket GetSocket()
        {
            return mySocket;
        }

        private Socket connectServer(string IP, int port)
        {
            IPAddress ip = IPAddress.Parse(IP);
            IPEndPoint ipe = new IPEndPoint(ip, port);//把ip和端口转化为IPEndpoint实例

            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建Socket

            try
            {
                sock.Connect(ipe);//连接到服务器
            }
            catch (Exception)
            {
                //MessageBoxEx.Show("无法连接服务器(" + IP + ":" + port.ToString() + ")");
            }

            return sock;
        }
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void Form_login_FormClosed(object sender, FormClosedEventArgs e)
        {
            //this.DialogResult = DialogResult.No;
        }

        private void button_login_Click(object sender, EventArgs e)
        {
            string account = textbox_account.Text.Trim();
            string password = textbox_password.Text.Trim();

            if (mySocket.Connected)
            {
                if (account.Length == 0)
                {
                    MessageBoxEx.Show("账号不能为空");
                    return;
                }
                if (password.Length == 0)
                {
                    MessageBoxEx.Show("密码不能为空");
                    return;
                }

                string sendString = (int)CMD.CMD_ACCOUNT_LOGIN + ";" + account + ";" + password;
                int cmd = (int)CMD.CMD_ACCOUNT_LOGIN;

                //send buffer to server
                lock (myParent.getCmdLock(cmd))
                {
                    byte[] bs = Encoding.UTF8.GetBytes(sendString);//把字符串编码为字节
                    mySocket.Send(bs, bs.Length, 0); //lock and then send buffer
                    bool noTimeOut = Monitor.Wait(myParent.getCmdLock(cmd), 5000);
                    if (noTimeOut)
                    {
                        if (myParent.getReplyMessage(cmd).getReply() == (int)REPLY.REPLY_OK)
                        {
                            login_account = account;
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else if (myParent.getReplyMessage(cmd).getReply() == (int)REPLY.REPLY_ERROR_ACCOUNT)
                        {
                            MessageBoxEx.Show("未注册的账号");
                        }
                        else if (myParent.getReplyMessage(cmd).getReply() == (int)REPLY.REPLY_ERROR_PASSWORD)
                        {
                            MessageBoxEx.Show("密码错误");
                        }
                        else
                        {
                            MessageBoxEx.Show("发生未知错误");
                        }
                    }
                    else
                    {
                        MessageBoxEx.Show("登录超时");
                    }
                }
            }
            else
            {
                MessageBoxEx.Show("无法连接服务器");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!mySocket.Connected)
                return;

            Form form_register = new Form_register(mySocket, myParent);
            form_register.ShowDialog();
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form_about().ShowDialog();
        }

        private void 服务器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form_serverInfo().ShowDialog();
        }
    }
}
