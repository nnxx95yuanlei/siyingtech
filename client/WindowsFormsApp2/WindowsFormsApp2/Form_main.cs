using System;
using System.Collections;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp2
{

    public partial class Form_main : Form
    {
        private string login_account = "";
        private string myUsername = "";
        private string mySex = "";
        private string myState = "";
        private Socket mySocket;

        private readonly object[] cmd_lock = new object[(int)CMD.CMD_MAX];
        private ReplyMessage[] replyMsg = new ReplyMessage[(int)CMD.CMD_MAX];
        private readonly object chatfile_lock = new object();
        private readonly object process_lock = new object();
        private readonly object ready_process_lock = new object();
        private bool ready_process = false;
        private Queue messageQueue = new Queue();

        private ContextMenuStrip listview_rightMenu;
        private ContextMenuStrip listview_rightMenu_Temp;

        public delegate void DelegateUpdateDisplay(string friend_account);
        public delegate void DelegateUpdateTempFriend(string friend_account);

        public Form_main()
        {
            InitializeComponent();

            richTextBox_receive.Clear();
            richTextBox_send.Clear();
        }

        public Form_main(string account, Socket socket)
        {
            InitializeComponent();
            login_account = account;
            mySocket = socket;

            richTextBox_receive.Clear();
            richTextBox_send.Clear();

            if (!mySocket.Connected) return;

            for (int i = 0; i < cmd_lock.Length; i++)
            {
                cmd_lock[i] = new object();
            }

            Thread pollThread = new Thread(pollThreadFunc);
            pollThread.Start();

            Thread processThread = new Thread(processThreadFunc);
            processThread.Start();
        }

        public void setLoginAccount(string account)
        {
            login_account = account;
        }

        public object getCmdLock(int cmd)
        {
            return cmd_lock[cmd];
        }
        public ReplyMessage getReplyMessage(int cmd)
        {
            return replyMsg[cmd];
        }

        private void Form_main_Load(object sender, EventArgs e)
        {
            groupBox_myfriend.Text = "我的好友[" + login_account + "]";
            this.Text = "四鹰聊天室[" + login_account + "] - Powered by Lei Yuan";
            label_state.Text = "";
            label_from.Text = "";
            label_sendto.Text = "";
            groupBox_msg.Enabled = false;
            listview_rightMenu = new ContextMenuStrip();
            listview_rightMenu.Items.Add("详细信息");
            listview_rightMenu.Items[0].Click += new System.EventHandler(rightMenu_showDetail_Click);

            listview_rightMenu_Temp = new ContextMenuStrip();
            listview_rightMenu_Temp.Items.Add("详细信息");
            listview_rightMenu_Temp.Items.Add("加为好友");
            listview_rightMenu_Temp.Items.Add("删除");
            listview_rightMenu_Temp.Items[0].Click += new System.EventHandler(rightMenu_showDetail_Click);
            listview_rightMenu_Temp.Items[1].Click += new System.EventHandler(rightMenu_addTempFriend_Click);
            listview_rightMenu_Temp.Items[2].Click += new System.EventHandler(rightMenu_delTempFriend_Click);

            string[] login_detail_array = getAccountDetail(login_account).Split(';');
            if (login_detail_array.Length >= 4)
            {
                myUsername = login_detail_array[1];
                mySex = login_detail_array[2];
                myState = login_detail_array[3];
                groupBox_myfriend.Text = "我的好友[" + myUsername + "]";
            }

            listFriends();

            lock(ready_process_lock)
            {
                ready_process = true;
                Monitor.Pulse(ready_process_lock);
            }
        }
        private void processThreadFunc()
        {
            Control.CheckForIllegalCrossThreadCalls = false;

            lock(ready_process_lock)
            {
                while(ready_process == false)
                {
                    Monitor.Wait(ready_process_lock);
                }
            }
            //MessageBox.Show("yuanlei start process");
            while (true)
            {
                lock(process_lock)
                {
                    if(messageQueue.Count <= 0)
                    {
                        Monitor.Wait(process_lock);
                    }
                }
                string msg = (string)messageQueue.Dequeue();
                if (msg.Equals("")) continue;

                RichTextBox richTextBox_temp2 = new RichTextBox();
                //process received msg
                string from = msg.Split(';')[0];
                string time = msg.Split(';')[1];
                string tmp = msg.Substring(msg.IndexOf(';') + 1, msg.Length - from.Length - 1);
                string data = tmp.Substring(tmp.IndexOf(';') + 1, tmp.Length - time.Length - 1);

                if (from.Equals("")) continue;

                lock (chatfile_lock)
                {
                    loadConversation(richTextBox_temp2, from);

                    richTextBox_temp2.Text += "(" + time + ") ";
                    richTextBox_temp2.Text += from + "  :";
                    richTextBox_temp2.Text += System.Environment.NewLine;
                    richTextBox_temp2.Text += data;
                    richTextBox_temp2.Text += System.Environment.NewLine;

                    //set format
                    //MessageBoxEx.Show("1   :" + (temp.Text.Length - data.Length) + "  " + data.Length + "   " +data);
                    //temp.Select(temp.Text.Length - data.Length, data.Length);
                    //temp.SelectionFont = new Font(FontFamily.GenericMonospace, 14, FontStyle.Regular);
                    //temp.SelectionColor = Color.Red;
                    //temp.SelectionAlignment = HorizontalAlignment.Left;

                    saveConversation(richTextBox_temp2, from);
                }
                listView_friends.Invoke(new DelegateUpdateTempFriend(updateTempFriend), new object[] { from });
                richTextBox_receive.Invoke(new DelegateUpdateDisplay(updateReceiveBox), new object[] { from });
            }
        }

        private void updateTempFriend(string tempFriend_Account)
        {
            for(int i = 0; i < listView_friends.Items.Count; i++)
            {
                if(listView_friends.Items[i].SubItems[2].Text.Equals(tempFriend_Account))
                {
                    int missMessages = int.Parse(listView_friends.Items[i].SubItems[5].Text) + 1;
                    listView_friends.Items[i].SubItems[5].Text = missMessages.ToString();
                    listView_friends.Items[i].SubItems[1].Text = listView_friends.Items[i].SubItems[5].Text + "条未读"; //update state
                    listView_friends.Items[i].SubItems[1].ForeColor = Color.Red;
                    return;
                }
            }

            ListViewItem item = new ListViewItem();
            item.Text = tempFriend_Account + "(右击加好友)";                      //0
            item.SubItems.Add("1条未读");     //1
            item.SubItems[1].ForeColor = Color.Red;
            item.SubItems.Add(tempFriend_Account);        //reserve account to column[2]     //2
            item.SubItems.Add("未知");      //3  store state
            item.SubItems[3].ForeColor = Color.Red;
            item.SubItems.Add("1");                       //4  0:friend 1:temp
            item.SubItems.Add("1");                       //5  number messages
            item.UseItemStyleForSubItems = false;
            item.BackColor = Color.LightGray;
            listView_friends.Items.Add(item);
        }

        private string getAccountDetail(string account)
        {
            string sendString = (int)CMD.CMD_GET_ACCOUNT_DETAIL_INFO + ";" + login_account + ";" + account;
            int cmd = (int)CMD.CMD_GET_ACCOUNT_DETAIL_INFO;
            string detailInfo = "";

            //send buffer to server
            lock (cmd_lock[cmd])
            {
                byte[] bs = Encoding.UTF8.GetBytes(sendString);//把字符串编码为字节
                mySocket.Send(bs, bs.Length, 0); //lock and then send buffer
                bool noTimeOut = Monitor.Wait(cmd_lock[cmd], 5000);
                if (noTimeOut)
                {
                    if (replyMsg[cmd].getReply() == (int)REPLY.REPLY_OK)
                    {
                        detailInfo = getReplyMessage(cmd).getMsg();
                    }
                }
            }

            return detailInfo;
        }

        private void rightMenu_showDetail_Click(object sender, EventArgs e)
        {
            //show detail
            string account = listView_friends.Items[listView_friends.SelectedIndices[0]].SubItems[2].Text;
            string detailInfo = getAccountDetail(account);

            Form detail = new Form_detailInfo(detailInfo);
            detail.ShowDialog();
        }

        private void rightMenu_addTempFriend_Click(object sender, EventArgs e)
        {
            //show detail， and get account name
            string account = listView_friends.Items[listView_friends.SelectedIndices[0]].SubItems[2].Text;
            string detailInfo = getAccountDetail(account);

            string[] detailArray = detailInfo.Split(';');
            if (detailArray.Length < 4) return;

            string accountName = detailArray[1];

            //add friend
            string sendString = (int)CMD.CMD_ADD_FRIENDS + ";" + login_account + ";" +
                  account + ";" +
                  accountName;
            int cmd = (int)CMD.CMD_ADD_FRIENDS;

            lock (cmd_lock[cmd])
            {
                byte[] bs = Encoding.UTF8.GetBytes(sendString);//把字符串编码为字节
                mySocket.Send(bs, bs.Length, 0); //lock and then send buffer
                bool noTimeOut = Monitor.Wait(cmd_lock[cmd], 5000);
                if (noTimeOut)
                {
                    if (replyMsg[cmd].getReply() == (int)REPLY.REPLY_OK)
                    {

                    }
                    else if (replyMsg[cmd].getReply() == (int)REPLY.REPLY_ERROR_ACCOUNT)
                    {
                        MessageBoxEx.Show("账号未登录，无法添加好友");
                        return;
                    }
                    else
                    {
                        MessageBoxEx.Show("添加好友失败,未知错误");
                        return;
                    }
                }
                else
                {
                    MessageBoxEx.Show("添加好友超时");
                    return;
                }
            }

            listFriends();
        }

        private void rightMenu_delTempFriend_Click(object sender, EventArgs e)
        {
            listView_friends.Items[listView_friends.SelectedIndices[0]].Remove();
        }

        private void pollThreadFunc()
        {
            Control.CheckForIllegalCrossThreadCalls = false;

            while (true)
            {
                byte[] recvbuf = new byte[4096];
                int count = mySocket.Receive(recvbuf);
                string[] recvdataArray = Encoding.UTF8.GetString(recvbuf, 0, count).Split('\0');

                for(int i = 0; i < recvdataArray.Length - 1; i++)
                {
                    string recvdata = recvdataArray[i];
                    string[] dataArray = recvdata.Split(";".ToCharArray());
                    int reply = int.Parse(dataArray[0].ToString());
                    int cmd = int.Parse(dataArray[1].ToString());

                    int cmdPos = recvdata.IndexOf(';');
                    int dataPos = recvdata.IndexOf(';', cmdPos + 1, recvdata.Length - cmdPos - 1);
                    string dataStr = recvdata.Substring(dataPos + 1);
                    //MessageBox.Show(recvdata);
                    if (cmd <= (int)CMD.CMD_NULL || cmd >= (int)CMD.CMD_MAX)
                    {
                        continue;
                    }

                    if (reply >= (int)REPLY.REPLY_HAVE_MESSAGE)
                    {
                        switch (reply)
                        {
                            case (int)REPLY.REPLY_HAVE_MESSAGE:
                                lock (process_lock)
                                {
                                    messageQueue.Enqueue(dataStr);
                                    Monitor.Pulse(process_lock);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        lock (cmd_lock[cmd])
                        {
                            replyMsg[cmd] = new ReplyMessage(cmd, reply, dataStr);
                            Monitor.Pulse(cmd_lock[cmd]);
                        }
                    }
                }
            }
        }
        private void updateReceiveBox(string friend_account)
        {
            if (listView_friends.SelectedIndices != null && listView_friends.SelectedIndices.Count > 0)
            {
                if (listView_friends.Items[listView_friends.SelectedIndices[0]].SubItems[2].Text.Equals(friend_account))
                {
                    loadConversation(richTextBox_receive, friend_account);
                }
            }
        }
 

        private void loadConversation(RichTextBox box, string friend_account)
        {
            string fileDirectory = "./." + login_account;
            if (!System.IO.Directory.Exists(fileDirectory))
            {
                System.IO.Directory.CreateDirectory(fileDirectory);
                System.IO.File.SetAttributes(fileDirectory, System.IO.File.GetAttributes(fileDirectory) | System.IO.FileAttributes.Hidden);
            }
            string fileName = fileDirectory + "/" + friend_account + ".rtf";
            if (!System.IO.File.Exists(fileName))
            {
                System.IO.File.Create(fileName).Close();
                RichTextBox temp = new RichTextBox();
                temp.Text = "==========================================TOP=============================================";
                temp.Text += System.Environment.NewLine;
                temp.Select(0, temp.Text.Length);
                temp.SelectionFont = new Font("宋体", 12);
                temp.SelectionColor = Color.MidnightBlue;
                temp.SaveFile(fileName);
            }

            box.Clear();
            box.LoadFile(fileName);

            box.SelectionStart = richTextBox_receive.Text.Length;
            box.ScrollToCaret();
        }

        private void saveConversation(RichTextBox box, string friend_account)
        {
            string fileDirectory = "./." + login_account;
            if (!System.IO.Directory.Exists(fileDirectory))
            {
                System.IO.Directory.CreateDirectory(fileDirectory);
                System.IO.File.SetAttributes(fileDirectory, System.IO.File.GetAttributes(fileDirectory) | System.IO.FileAttributes.Hidden);
            }
            string fileName = fileDirectory + "/" + friend_account + ".rtf";
            if (!System.IO.File.Exists(fileName))
            {
                System.IO.File.Create(fileName).Close();
                RichTextBox temp = new RichTextBox();
                temp.Text = "==========================================TOP=============================================";
                temp.Text += System.Environment.NewLine;
                temp.Select(0, temp.Text.Length);
                temp.SelectionFont = new Font("宋体", 12);
                temp.SelectionColor = Color.MidnightBlue;
                temp.SaveFile(fileName);
            }

            box.SaveFile(fileName);
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            Form form_addFriend = new Form_addFriend(this, login_account, mySocket);
            form_addFriend.ShowDialog();
            // list the friends
            listFriends();
        }

        private void listFriends()
        {
            int friends_num = 0;
            string friendStr;
            int ret = getFriends(out friendStr);
            if(ret == 0)
            {
                string[] friendArray = friendStr.Split(';');

                for (int i = 0; i < friendArray.Length; i += 3)
                {
                    if ((i + 3) >= friendArray.Length) break;

                    //find and update
                    bool found = false;
                    for(int j = 0; j < listView_friends.Items.Count; j++)
                    {
                        //update
                        if (listView_friends.Items[j].SubItems[2].Text.Equals(friendArray[i].ToString()))
                        {
                            listView_friends.Items[j].SubItems[0].Text = friendArray[i + 1].ToString();
                            listView_friends.Items[j].SubItems[0].BackColor = Color.White;
                            if (listView_friends.Items[j].SubItems[1].Text.IndexOf("未读") < 0)
                            {
                                listView_friends.Items[j].SubItems[1].Text = friendArray[i + 2].ToString().Equals("online") ? "在线" : "离线";
                                listView_friends.Items[j].SubItems[1].ForeColor = friendArray[i + 2].ToString().Equals("online") ? Color.Green : Color.Gray;
                            }
                            listView_friends.Items[j].SubItems[2].Text = friendArray[i].ToString();
                            listView_friends.Items[j].SubItems[3].Text = friendArray[i + 2].ToString().Equals("online") ? "在线" : "离线";
                            listView_friends.Items[j].SubItems[3].ForeColor  = friendArray[i + 2].ToString().Equals("online") ? Color.Green : Color.Gray;
                            listView_friends.Items[j].SubItems[4].Text = "0";

                            found = true;
                            friends_num++;
                            break;
                        }
                    }
                    if (found) continue;

                    ListViewItem item = new ListViewItem();
                    item.Text = friendArray[i+1].ToString();                      //0
                    item.SubItems.Add(friendArray[i+2].ToString().Equals("online") ? "在线" : "离线");     //1
                    item.SubItems[1].ForeColor = friendArray[i + 2].ToString().Equals("online") ? Color.Green : Color.Gray;
                    item.SubItems.Add(friendArray[i].ToString()); //reserve account to column[2]     //2
                    item.SubItems.Add(item.SubItems[1].Text);      //3  store state
                    item.SubItems[3].ForeColor = item.SubItems[1].ForeColor;
                    item.SubItems.Add("0");                       //4  0:friend 1:temp
                    item.SubItems.Add("0");                       //5  number messages
                    item.UseItemStyleForSubItems = false;
                    listView_friends.Items.Add(item);
                    friends_num++;
                }
                label_state.Text = "好友 " + friends_num.ToString() + " 位";
                label_state.ForeColor = Color.Green;
            } else if(ret == 2){
                label_state.Text = "账号未登录，无法获取好友列表";
                label_state.ForeColor = Color.Red;
            } else
            {
                label_state.Text = "获取好友列表失败，未知错误";
                label_state.ForeColor = Color.Red;
            }
        }

        private void clearFriends()
        {
            for(int i = 0; i < listView_friends.Items.Count; i++)
            {
                if(listView_friends.Items[i].SubItems[4].Text.Equals("0"))
                {
                    listView_friends.Items[i].Remove();
                    i--;
                }
            }
        }

        private int getFriends(out string friends_list)
        {
            string sendString = (int)CMD.CMD_GET_FRIENDS + ";" + login_account;
            byte[] bs = Encoding.UTF8.GetBytes(sendString);//把字符串编码为字节
            int cmd = (int)CMD.CMD_GET_FRIENDS;
            int ret = -1;

            lock (cmd_lock[cmd])
            {
                mySocket.Send(bs, bs.Length, 0); //lock and then send buffer
                bool noTimeOut = Monitor.Wait(cmd_lock[cmd], 5000);
                if(noTimeOut)
                {
                    if (replyMsg[cmd].getReply() == (int)REPLY.REPLY_OK)
                    {
                        friends_list = replyMsg[cmd].getMsg();
                        return 0;
                    } else
                    {
                        ret = replyMsg[cmd].getReply();
                    }
                }
                else
                {
                    MessageBoxEx.Show("连接超时");

                }
            }

            friends_list = "";
            return ret;
        }

        private void button_refresh_Click(object sender, EventArgs e)
        {
            listFriends();
        }

        private void listView_friends_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                groupBox_msg.Enabled = true;
                label_sendto.Text = "发消息给 " + e.Item.Text + " ：";
                label_sendto.ForeColor = Color.Green;
                label_from.Text = "来自 " + e.Item.Text + " 的消息：";
                label_from.ForeColor = Color.Green;

                e.Item.SubItems[1].Text = e.Item.SubItems[3].Text;
                e.Item.SubItems[1].ForeColor = e.Item.SubItems[3].ForeColor;
                e.Item.SubItems[5].Text = "0";

                loadConversation(richTextBox_receive, e.Item.SubItems[2].Text);
            }
            else
            {
                disableSendMessage();
            }
        }

        private void disableSendMessage()
        {
            groupBox_msg.Enabled = false;
            label_sendto.Text = "";
            label_from.Text = "";

            richTextBox_receive.Clear();
        }

        private void button_send_Click(object sender, EventArgs e)
        {
            if(richTextBox_send.Text.Equals(""))
            {
                MessageBoxEx.Show("发送内容为空");
                return;
            }

            if (listView_friends.SelectedIndices == null || listView_friends.SelectedIndices.Count <= 0)
            {
                MessageBoxEx.Show("收件人错误");
                return;
            }

            string to = listView_friends.Items[listView_friends.SelectedIndices[0]].SubItems[2].Text;
            string sendString = (int)CMD.CMD_SEND_MSG + ";" + login_account + ";" + to + ";" + richTextBox_send.Text;
            int cmd = (int)CMD.CMD_SEND_MSG;
            RichTextBox richTextBox_temp = new RichTextBox();

            //update display
            lock (chatfile_lock)
            {
                loadConversation(richTextBox_temp, to);

                int startPos = richTextBox_temp.Text.Length;
                richTextBox_temp.Text += "(" + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + ") ";
                richTextBox_temp.Text += login_account + " (我)" + " :";
                richTextBox_temp.Text += System.Environment.NewLine;
                richTextBox_temp.Text += richTextBox_send.Text;
                richTextBox_temp.Text += System.Environment.NewLine;
                //set format
                //MessageBoxEx.Show("2   :" + (richTextBox_temp.Text.Length - richTextBox_send.Text.Length) + "  " + richTextBox_send.Text.Length);
                //richTextBox_temp.Select(startPos, richTextBox_temp.Text.Length - startPos);
                //richTextBox_temp.SelectionFont = new Font(FontFamily.GenericMonospace, 14, FontStyle.Regular);
                //richTextBox_temp.SelectionColor = Color.Green;
                //richTextBox_temp.SelectionAlignment = HorizontalAlignment.Right;

                saveConversation(richTextBox_temp, to);
            }

            updateReceiveBox(to);

            //send buffer to server
            lock (cmd_lock[cmd])
            {
                byte[] bs = Encoding.UTF8.GetBytes(sendString);//把字符串编码为字节
                mySocket.Send(bs, bs.Length, 0); //lock and then send buffer
                bool noTimeOut = Monitor.Wait(cmd_lock[cmd], 5000);
                if (noTimeOut)
                {
                    if (replyMsg[cmd].getReply() == (int)REPLY.REPLY_OK)
                    {
                        //MessageBoxEx.Show("发送成功");
                        richTextBox_send.Clear();
                    }
                    else
                    {
                        MessageBoxEx.Show("发送失败 ret=" + replyMsg[cmd].getReply().ToString());
                    }
                }
                else
                {
                    MessageBoxEx.Show("发送失败");
                }
            }
        }

        private void Form_main_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void listView_friends_Leave(object sender, EventArgs e)
        {

        }

        private void listView_friends_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox_send_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == (Keys.Control | Keys.Enter))
            {
                richTextBox_send.Text.Insert(richTextBox_send.SelectionStart, System.Environment.NewLine);
            }
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            if (listView_friends.SelectedIndices != null && listView_friends.SelectedIndices.Count > 0)
            {
                string friend_account = listView_friends.Items[listView_friends.SelectedIndices[0]].SubItems[2].Text;
                richTextBox_receive.Text = "==========================================TOP=============================================";
                richTextBox_receive.Text += System.Environment.NewLine;
                richTextBox_receive.Select(0, richTextBox_receive.Text.Length);
                richTextBox_receive.SelectionFont = new Font("宋体", 12);
                richTextBox_receive.SelectionColor = Color.MidnightBlue;
                saveConversation(richTextBox_receive, friend_account);
            }
        }

        private void richTextBox_receive_TextChanged(object sender, EventArgs e)
        {
            richTextBox_receive.SelectionStart = richTextBox_receive.Text.Length;
            richTextBox_receive.ScrollToCaret();
        }

        private void richTextBox_send_TextChanged(object sender, EventArgs e)
        {
            richTextBox_send.SelectionStart = richTextBox_send.Text.Length;
            richTextBox_send.ScrollToCaret();
        }

        private void listView_friends_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                if (listView_friends.Items[listView_friends.SelectedIndices[0]].SubItems[4].Text.Equals("0"))
                {
                    listview_rightMenu.Show(listView_friends, e.Location);
                } else
                {
                    listview_rightMenu_Temp.Show(listView_friends, e.Location);
                }
                
            }
        }

        private void listView_friends_ItemActivate(object sender, EventArgs e)
        {

        }

        private void 我的资料ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string detailInfo = login_account + ";" + myUsername + ";" + mySex + ";" + myState;
            Form detail = new Form_detailInfo(detailInfo);
            detail.ShowDialog();
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form_about().ShowDialog();
        }

        private void Form_main_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
