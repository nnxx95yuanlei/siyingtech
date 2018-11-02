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
    public partial class Form_addFriend : Form
    {
        private string login_account;
        private ContextMenuStrip rightMenu = new ContextMenuStrip();
        Socket mySocket;
        private Form_main parent;

        public Form_addFriend()
        {
            InitializeComponent();
        }

        public Form_addFriend(Form_main parent_form, string account, Socket socket)
        {
            InitializeComponent();
            login_account = account;
            mySocket = socket;
            parent = parent_form;
        }

        private void Form_addFriend_Load(object sender, EventArgs e)
        {
            label_state.Text = "";
            button_add.Enabled = false;

            listView_searchFriend.Columns[0].Width = 148;
            listView_searchFriend.Columns[1].Width = 155;
            listView_searchFriend.Columns[2].Width = 85;
            listView_searchFriend.Columns[3].Width = 78;

            rightMenu.Items.Add("添加");
            rightMenu.Items[0].Click += new System.EventHandler(rightMenu_add_Click);
        }

        private void rightMenu_add_Click(object sender, EventArgs e)
        {
            button_add_Click(sender, e);
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
                label_err.Text = "不能包含除特殊字符";
                label_err.ForeColor = Color.Red;
                return false;
            }

            label_err.Text = "";
            return true;
        }

        private bool search_account(string search_account, out string outStr)
        {
            string sendString = (int)CMD.CMD_FIND_ACCOUNTS + ";" + login_account + ";" + search_account;
            int cmd = (int)CMD.CMD_FIND_ACCOUNTS;

            lock (parent.getCmdLock(cmd))
            {
                byte[] bs = Encoding.UTF8.GetBytes(sendString);//把字符串编码为字节
                mySocket.Send(bs, bs.Length, 0); //lock and then send buffer
                bool noTimeOut = Monitor.Wait(parent.getCmdLock(cmd), 5000);
                if (noTimeOut)
                {
                    if (parent.getReplyMessage(cmd).getReply() == (int)REPLY.REPLY_OK)
                    {
                        outStr = parent.getReplyMessage(cmd).getMsg();
                        return true;
                    }
                    else
                    {
                        label_state.Text = "搜索发生错误";
                        label_state.ForeColor = Color.Red;
                        outStr = "";
                    }
                }
                else
                {
                    label_state.Text = "搜索超时";
                    label_state.ForeColor = Color.Red;
                    outStr = "";
                }
            }

            return false;
        }
        private void button_searchall_Click(object sender, EventArgs e)
        {
            string accountStr;
            bool success = search_account("", out accountStr);
            if(success)
            {
                int num = listFriend(accountStr, listView_searchFriend);
                label_state.Text = "搜索到" + num + "个用户";
                label_state.ForeColor = Color.Green;
            }
        }

        private void button_search_Click(object sender, EventArgs e)
        {
            bool valid = checkStringValid(textBox_search.Text.Trim(), label_state);
            if (!valid) return;

            string accountStr;
            bool success = search_account(textBox_search.Text.Trim(), out accountStr);
            if (success)
            {
                int num = listFriend(accountStr, listView_searchFriend);
                label_state.Text = "搜索到" + num + "个用户";
                label_state.ForeColor = Color.Green;
            }
        }

        private void textBox_search_Leave(object sender, EventArgs e)
        {
            //checkStringValid(textBox_search.Text.Trim(), label_state);
        }

        private int listFriend(string friendStr, ListView list)
        {
            int friendNum = 0;
            string[] friendArray = friendStr.Split(';');
            list.Items.Clear();
            for (int i = 0; i < friendArray.Length; i+=4)
            {
                if ((i + 4) >= friendArray.Length) break;
                ListViewItem item = new ListViewItem();
                item.Text = friendArray[i].ToString();
                item.SubItems.Add(friendArray[i + 1].ToString());
                item.SubItems.Add(friendArray[i + 2].ToString());
                item.SubItems.Add(friendArray[i + 3].ToString().Equals("online") ? "在线" : "离线");
                item.SubItems[3].ForeColor = friendArray[i + 3].ToString().Equals("online") ? Color.Green : Color.Gray;
                item.UseItemStyleForSubItems = false;
                list.Items.Add(item);
                friendNum++;
            }

            return friendNum;
        }

        private void listView_searchFriend_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if(e.IsSelected)
            {
                button_add.Enabled = true;
            } else
            {
                button_add.Enabled = false;
            }
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            if(listView_searchFriend.SelectedIndices != null && listView_searchFriend.SelectedIndices.Count > 0)
            {
                for (int i = 0; i < listView_searchFriend.SelectedIndices.Count; i++)
                {
                    string sendString = (int)CMD.CMD_ADD_FRIENDS + ";" + login_account + ";" +
                                     listView_searchFriend.Items[listView_searchFriend.SelectedIndices[i]].Text + ";" +
                                     listView_searchFriend.Items[listView_searchFriend.SelectedIndices[i]].SubItems[1].Text;
                    int cmd = (int)CMD.CMD_ADD_FRIENDS;

                    lock (parent.getCmdLock(cmd))
                    {
                        byte[] bs = Encoding.UTF8.GetBytes(sendString);//把字符串编码为字节
                        mySocket.Send(bs, bs.Length, 0); //lock and then send buffer
                        bool noTimeOut = Monitor.Wait(parent.getCmdLock(cmd), 5000);
                        if (noTimeOut)
                        {
                            if (parent.getReplyMessage(cmd).getReply() == (int)REPLY.REPLY_OK)
                            {
                                string addedFriend = listView_searchFriend.Items[listView_searchFriend.SelectedIndices[i]].Text;
                                MessageBoxEx.Show("成功添加 " + addedFriend + " 为好友");
                            }
                            else if (parent.getReplyMessage(cmd).getReply() == (int)REPLY.REPLY_ERROR_ACCOUNT)
                            {
                                MessageBoxEx.Show("账号未登录，无法添加好友");
                            }
                            else
                            {
                                MessageBoxEx.Show("添加好友失败,未知错误");
                            }
                        }
                        else
                        {
                            MessageBoxEx.Show("添加好友超时");
                        }
                    }
                }
            }
        }

        private void listView_searchFriend_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                rightMenu.Show(listView_searchFriend, e.Location);
            }
        }
    }
}
