using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form_register : Form
    {
        private Socket mySocket;
        private Form_main myParent;

        public Form_register()
        {
            InitializeComponent();
        }

        public Form_register(Socket socket, Form_main parent)
        {
            InitializeComponent();
            mySocket = socket;
            myParent = parent;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Form_register_Load(object sender, EventArgs e)
        {
            label_account.Text = "";
            label_password.Text = "";
            label_password2.Text = "";
            label_username.Text = "";
            label_sex.Text = "";
            comboBox_sex.SelectedIndex = 0;
        }

        private bool checkStringValid(string str, Label label_err)
        {
            if (str.Equals(""))
            {
                label_err.Text = "不能为空";
                label_err.ForeColor = Color.Red;
                return false;
            }

            if (str.IndexOf(' ') >= 0)
            {
                label_err.Text = "不能包含空格";
                label_err.ForeColor = Color.Red;
                return false;
            }

            string pattern = @"^[a-zA-Z0-9]*$"; //匹配所有字符都在字母和数字之间
            if (!System.Text.RegularExpressions.Regex.IsMatch(str, pattern))
            {
                label_err.Text = "不能包含除a-z A-Z 0-9之外的字符";
                label_err.ForeColor = Color.Red;
                return false;
            }

            if(str.Length > 48)
            {
                label_err.Text = "不能超过48个字符";
                label_err.ForeColor = Color.Red;
                return false;
            }

            label_err.Text = "";
            return true;
        }

        private void button_checkAccount_Click(object sender, EventArgs e)
        {
            bool valid = checkStringValid(textBox_account.Text, label_account);
            if (!valid) return;

            //check account
            string sendString = (int)CMD.CMD_CHECK_ACCOUNT + ";" + textBox_account.Text;
            int cmd = (int)CMD.CMD_CHECK_ACCOUNT;

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
                        label_account.Text = "账号可用";
                        label_account.ForeColor = Color.Green;
                    }
                    else if (myParent.getReplyMessage(cmd).getReply() == (int)REPLY.REPLY_ERROR_ACCOUNT)
                    {
                        label_account.Text = "账号已被注册，请更换";
                        label_account.ForeColor = Color.Red;
                    }
                    else
                    {
                        label_account.Text = "未知错误";
                        label_account.ForeColor = Color.Red;
                    }
                }
                else
                {
                    label_account.Text = "查询超时，服务器异常";
                    label_account.ForeColor = Color.Red;
                }
            }
        }


        private void textBox_account_Leave(object sender, EventArgs e)
        {
            bool valid = checkStringValid(textBox_account.Text, label_account);
            if(valid)
            {
                label_account.Text = "";
            }
        }

        private void textBox_password_Leave(object sender, EventArgs e)
        {
            bool valid = checkStringValid(textBox_password.Text, label_password);
            if(valid)
            {
                if(textBox_password.Text.Length < 6 || textBox_password.Text.Length > 16)
                {
                    label_password.Text = "密码长度有误";
                    label_password.ForeColor = Color.Red;
                } else
                {
                    label_password.Text = "";
                }
            }
        }

        private void textBox_password2_Leave(object sender, EventArgs e)
        {
            if(textBox_password.Text.Equals(textBox_password2.Text))
            {
                label_password2.Text = "";
            } else
            {
                label_password2.Text = "密码不匹配";
                label_password.ForeColor = Color.Red;
            }
        }

        private void textBox_username_Leave(object sender, EventArgs e)
        {
            if (textBox_username.Text.Equals(""))
            {
                label_username.Text = "用户名不能为空";
                label_username.ForeColor = Color.Red;
                return;
            }

            if (textBox_username.Text.IndexOf(' ') >= 0)
            {
                label_username.Text = "不能包含空格";
                label_username.ForeColor = Color.Red;
                return;
            }
            if (textBox_username.Text.Length > 16)
            {
                label_username.Text = "用户名太长";
                label_username.ForeColor = Color.Red;
                return;
            }
            label_username.Text = "";
        }

        private void comboBox_sex_Leave(object sender, EventArgs e)
        {
            if (comboBox_sex.SelectedIndex < 0 || comboBox_sex.SelectedIndex > 3)
            {
                label_sex.Text = "性别有误";
                label_sex.ForeColor = Color.Red;
                return;
            }

            label_sex.Text = "";
        }

        private void button_register_Click(object sender, EventArgs e)
        {
            //check account
            bool valid = checkStringValid(textBox_account.Text, label_account);
            if (!valid) return;
            //check password
            valid = checkStringValid(textBox_password.Text, label_password);
            if (!valid) return;
            if (textBox_password.Text.Length < 6 || textBox_password.Text.Length > 16)
            {
                label_password.Text = "密码长度有误";
                label_password.ForeColor = Color.Red;
                return;
            }
            //check password again
            if (!textBox_password.Text.Equals(textBox_password2.Text))
            {
                label_password2.Text = "密码不匹配";
                label_password.ForeColor = Color.Red;
                return;
            }
            //check username
            if (textBox_username.Text.Equals(""))
            {
                label_username.Text = "用户名不能为空";
                label_username.ForeColor = Color.Red;
                return;
            }
            if (textBox_username.Text.IndexOf(' ') >= 0)
            {
                label_username.Text = "不能包含空格";
                label_username.ForeColor = Color.Red;
                return;
            }
            if (textBox_username.Text.Length > 16)
            {
                label_username.Text = "用户名太长";
                label_username.ForeColor = Color.Red;
                return;
            }
            //check sex
            if(comboBox_sex.SelectedIndex < 0 || comboBox_sex.SelectedIndex > 3)
            {
                label_sex.Text = "性别有误";
                label_sex.ForeColor = Color.Red;
                return;
            }
            //register to server
            //check account
            string sendString = (int)CMD.CMD_ACCOUNT_REGISTER + ";" + textBox_account.Text + ";" + textBox_password.Text + ";" + textBox_username.Text + ";"
                                + comboBox_sex.Items[comboBox_sex.SelectedIndex].ToString();
            int cmd = (int)CMD.CMD_ACCOUNT_REGISTER;

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
                        MessageBoxEx.Show("注册成功，用户名为" + textBox_account.Text);
                    }
                    else if (myParent.getReplyMessage(cmd).getReply() == (int)REPLY.REPLY_ERROR_ACCOUNT)
                    {
                        label_account.Text = "账号已被注册，请更换";
                        label_account.ForeColor = Color.Red;
                    }
                    else
                    {
                        label_account.Text = "未知错误";
                        label_account.ForeColor = Color.Red;
                    }
                }
                else
                {
                    label_account.Text = "注册超时，服务器异常";
                    label_account.ForeColor = Color.Red;
                }
            }
        }
    }
}
