namespace WindowsFormsApp2
{
    partial class Form_main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_main));
            this.button_refresh = new System.Windows.Forms.Button();
            this.groupBox_myfriend = new System.Windows.Forms.GroupBox();
            this.label_state = new System.Windows.Forms.Label();
            this.button_add = new System.Windows.Forms.Button();
            this.listView_friends = new System.Windows.Forms.ListView();
            this.username = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.state = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox_msg = new System.Windows.Forms.GroupBox();
            this.button_clear = new System.Windows.Forms.Button();
            this.label_from = new System.Windows.Forms.Label();
            this.label_sendto = new System.Windows.Forms.Label();
            this.button_send = new System.Windows.Forms.Button();
            this.richTextBox_send = new System.Windows.Forms.RichTextBox();
            this.richTextBox_receive = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.菜单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.我的资料ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox_myfriend.SuspendLayout();
            this.groupBox_msg.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_refresh
            // 
            this.button_refresh.Location = new System.Drawing.Point(20, 23);
            this.button_refresh.Name = "button_refresh";
            this.button_refresh.Size = new System.Drawing.Size(72, 25);
            this.button_refresh.TabIndex = 2;
            this.button_refresh.Text = "刷新";
            this.button_refresh.UseVisualStyleBackColor = true;
            this.button_refresh.Click += new System.EventHandler(this.button_refresh_Click);
            // 
            // groupBox_myfriend
            // 
            this.groupBox_myfriend.Controls.Add(this.label_state);
            this.groupBox_myfriend.Controls.Add(this.button_add);
            this.groupBox_myfriend.Controls.Add(this.listView_friends);
            this.groupBox_myfriend.Controls.Add(this.button_refresh);
            this.groupBox_myfriend.Location = new System.Drawing.Point(12, 37);
            this.groupBox_myfriend.Name = "groupBox_myfriend";
            this.groupBox_myfriend.Size = new System.Drawing.Size(265, 559);
            this.groupBox_myfriend.TabIndex = 1;
            this.groupBox_myfriend.TabStop = false;
            this.groupBox_myfriend.Text = "我的好友";
            // 
            // label_state
            // 
            this.label_state.AutoSize = true;
            this.label_state.Location = new System.Drawing.Point(7, 62);
            this.label_state.Name = "label_state";
            this.label_state.Size = new System.Drawing.Size(41, 12);
            this.label_state.TabIndex = 3;
            this.label_state.Text = "label1";
            // 
            // button_add
            // 
            this.button_add.Location = new System.Drawing.Point(149, 22);
            this.button_add.Name = "button_add";
            this.button_add.Size = new System.Drawing.Size(91, 25);
            this.button_add.TabIndex = 1;
            this.button_add.Text = "添加好友";
            this.button_add.UseVisualStyleBackColor = true;
            this.button_add.Click += new System.EventHandler(this.button_add_Click);
            // 
            // listView_friends
            // 
            this.listView_friends.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.username,
            this.state});
            this.listView_friends.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listView_friends.FullRowSelect = true;
            this.listView_friends.HideSelection = false;
            this.listView_friends.Location = new System.Drawing.Point(6, 82);
            this.listView_friends.MultiSelect = false;
            this.listView_friends.Name = "listView_friends";
            this.listView_friends.Size = new System.Drawing.Size(253, 470);
            this.listView_friends.TabIndex = 0;
            this.listView_friends.UseCompatibleStateImageBehavior = false;
            this.listView_friends.View = System.Windows.Forms.View.Details;
            this.listView_friends.ItemActivate += new System.EventHandler(this.listView_friends_ItemActivate);
            this.listView_friends.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listView_friends_ItemSelectionChanged);
            this.listView_friends.SelectedIndexChanged += new System.EventHandler(this.listView_friends_SelectedIndexChanged);
            this.listView_friends.Leave += new System.EventHandler(this.listView_friends_Leave);
            this.listView_friends.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView_friends_MouseClick);
            // 
            // username
            // 
            this.username.Text = "用户名";
            this.username.Width = 178;
            // 
            // state
            // 
            this.state.Text = "状态";
            this.state.Width = 70;
            // 
            // groupBox_msg
            // 
            this.groupBox_msg.Controls.Add(this.button_clear);
            this.groupBox_msg.Controls.Add(this.label_from);
            this.groupBox_msg.Controls.Add(this.label_sendto);
            this.groupBox_msg.Controls.Add(this.button_send);
            this.groupBox_msg.Controls.Add(this.richTextBox_send);
            this.groupBox_msg.Controls.Add(this.richTextBox_receive);
            this.groupBox_msg.Location = new System.Drawing.Point(310, 37);
            this.groupBox_msg.Name = "groupBox_msg";
            this.groupBox_msg.Size = new System.Drawing.Size(744, 559);
            this.groupBox_msg.TabIndex = 0;
            this.groupBox_msg.TabStop = false;
            this.groupBox_msg.Text = "聊天室";
            // 
            // button_clear
            // 
            this.button_clear.Location = new System.Drawing.Point(622, 13);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(102, 23);
            this.button_clear.TabIndex = 5;
            this.button_clear.Text = "清空聊天记录";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
            // 
            // label_from
            // 
            this.label_from.AutoSize = true;
            this.label_from.Location = new System.Drawing.Point(7, 23);
            this.label_from.Name = "label_from";
            this.label_from.Size = new System.Drawing.Size(59, 12);
            this.label_from.TabIndex = 4;
            this.label_from.Text = "labelfrom";
            // 
            // label_sendto
            // 
            this.label_sendto.AutoSize = true;
            this.label_sendto.Location = new System.Drawing.Point(7, 384);
            this.label_sendto.Name = "label_sendto";
            this.label_sendto.Size = new System.Drawing.Size(71, 12);
            this.label_sendto.TabIndex = 3;
            this.label_sendto.Text = "labelsendto";
            // 
            // button_send
            // 
            this.button_send.Location = new System.Drawing.Point(589, 520);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(141, 30);
            this.button_send.TabIndex = 1;
            this.button_send.Text = "发送(&Enter)";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.button_send_Click);
            // 
            // richTextBox_send
            // 
            this.richTextBox_send.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextBox_send.Location = new System.Drawing.Point(6, 399);
            this.richTextBox_send.MaxLength = 4000;
            this.richTextBox_send.Name = "richTextBox_send";
            this.richTextBox_send.Size = new System.Drawing.Size(732, 115);
            this.richTextBox_send.TabIndex = 0;
            this.richTextBox_send.Text = "";
            this.richTextBox_send.TextChanged += new System.EventHandler(this.richTextBox_send_TextChanged);
            this.richTextBox_send.KeyDown += new System.Windows.Forms.KeyEventHandler(this.richTextBox_send_KeyDown);
            // 
            // richTextBox_receive
            // 
            this.richTextBox_receive.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextBox_receive.Location = new System.Drawing.Point(6, 41);
            this.richTextBox_receive.Name = "richTextBox_receive";
            this.richTextBox_receive.ReadOnly = true;
            this.richTextBox_receive.Size = new System.Drawing.Size(732, 324);
            this.richTextBox_receive.TabIndex = 1;
            this.richTextBox_receive.Text = "";
            this.richTextBox_receive.TextChanged += new System.EventHandler(this.richTextBox_receive_TextChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.菜单ToolStripMenuItem,
            this.帮助ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1066, 25);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 菜单ToolStripMenuItem
            // 
            this.菜单ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.我的资料ToolStripMenuItem});
            this.菜单ToolStripMenuItem.Name = "菜单ToolStripMenuItem";
            this.菜单ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.菜单ToolStripMenuItem.Text = "菜单";
            // 
            // 我的资料ToolStripMenuItem
            // 
            this.我的资料ToolStripMenuItem.Name = "我的资料ToolStripMenuItem";
            this.我的资料ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.我的资料ToolStripMenuItem.Text = "我的资料";
            this.我的资料ToolStripMenuItem.Click += new System.EventHandler(this.我的资料ToolStripMenuItem_Click);
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.关于ToolStripMenuItem});
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.帮助ToolStripMenuItem.Text = "帮助";
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.关于ToolStripMenuItem.Text = "关于";
            this.关于ToolStripMenuItem.Click += new System.EventHandler(this.关于ToolStripMenuItem_Click);
            // 
            // Form_main
            // 
            this.AcceptButton = this.button_send;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1066, 606);
            this.Controls.Add(this.groupBox_msg);
            this.Controls.Add(this.groupBox_myfriend);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form_main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "四鹰聊天室 - Powered by Lei Yuan";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_main_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_main_FormClosed);
            this.Load += new System.EventHandler(this.Form_main_Load);
            this.groupBox_myfriend.ResumeLayout(false);
            this.groupBox_myfriend.PerformLayout();
            this.groupBox_msg.ResumeLayout(false);
            this.groupBox_msg.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_refresh;
        private System.Windows.Forms.GroupBox groupBox_myfriend;
        private System.Windows.Forms.Button button_add;
        private System.Windows.Forms.ListView listView_friends;
        private System.Windows.Forms.ColumnHeader username;
        private System.Windows.Forms.ColumnHeader state;
        private System.Windows.Forms.GroupBox groupBox_msg;
        private System.Windows.Forms.Button button_send;
        private System.Windows.Forms.RichTextBox richTextBox_send;
        private System.Windows.Forms.RichTextBox richTextBox_receive;
        private System.Windows.Forms.Label label_sendto;
        private System.Windows.Forms.Label label_state;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.Label label_from;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 菜单ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 我的资料ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
    }
}

