namespace WindowsFormsApp2
{
    partial class Form_addFriend
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_addFriend));
            this.textBox_search = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listView_searchFriend = new System.Windows.Forms.ListView();
            this.account = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.username = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button_search = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_add = new System.Windows.Forms.Button();
            this.label_state = new System.Windows.Forms.Label();
            this.button_searchall = new System.Windows.Forms.Button();
            this.state = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_search
            // 
            this.textBox_search.Location = new System.Drawing.Point(78, 28);
            this.textBox_search.Name = "textBox_search";
            this.textBox_search.Size = new System.Drawing.Size(113, 21);
            this.textBox_search.TabIndex = 0;
            this.textBox_search.Leave += new System.EventHandler(this.textBox_search_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "搜索账号：";
            // 
            // listView_searchFriend
            // 
            this.listView_searchFriend.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.account,
            this.username,
            this.sex,
            this.state});
            this.listView_searchFriend.FullRowSelect = true;
            this.listView_searchFriend.HideSelection = false;
            this.listView_searchFriend.Location = new System.Drawing.Point(7, 59);
            this.listView_searchFriend.MultiSelect = false;
            this.listView_searchFriend.Name = "listView_searchFriend";
            this.listView_searchFriend.Size = new System.Drawing.Size(472, 358);
            this.listView_searchFriend.TabIndex = 3;
            this.listView_searchFriend.UseCompatibleStateImageBehavior = false;
            this.listView_searchFriend.View = System.Windows.Forms.View.Details;
            this.listView_searchFriend.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listView_searchFriend_ItemSelectionChanged);
            this.listView_searchFriend.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView_searchFriend_MouseClick);
            // 
            // account
            // 
            this.account.Text = "账号";
            // 
            // username
            // 
            this.username.Text = "用户名";
            // 
            // sex
            // 
            this.sex.Text = "性别";
            this.sex.Width = 73;
            // 
            // button_search
            // 
            this.button_search.Location = new System.Drawing.Point(204, 26);
            this.button_search.Name = "button_search";
            this.button_search.Size = new System.Drawing.Size(58, 23);
            this.button_search.TabIndex = 1;
            this.button_search.Text = "搜索";
            this.button_search.UseVisualStyleBackColor = true;
            this.button_search.Click += new System.EventHandler(this.button_search_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_add);
            this.groupBox1.Controls.Add(this.label_state);
            this.groupBox1.Controls.Add(this.button_searchall);
            this.groupBox1.Controls.Add(this.listView_searchFriend);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button_search);
            this.groupBox1.Controls.Add(this.textBox_search);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(486, 458);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "搜索/添加好友";
            // 
            // button_add
            // 
            this.button_add.Location = new System.Drawing.Point(328, 424);
            this.button_add.Name = "button_add";
            this.button_add.Size = new System.Drawing.Size(131, 23);
            this.button_add.TabIndex = 3;
            this.button_add.Text = "加为好友";
            this.button_add.UseVisualStyleBackColor = true;
            this.button_add.Click += new System.EventHandler(this.button_add_Click);
            // 
            // label_state
            // 
            this.label_state.AutoSize = true;
            this.label_state.Location = new System.Drawing.Point(391, 32);
            this.label_state.Name = "label_state";
            this.label_state.Size = new System.Drawing.Size(41, 12);
            this.label_state.TabIndex = 5;
            this.label_state.Text = "label2";
            // 
            // button_searchall
            // 
            this.button_searchall.Location = new System.Drawing.Point(285, 26);
            this.button_searchall.Name = "button_searchall";
            this.button_searchall.Size = new System.Drawing.Size(87, 23);
            this.button_searchall.TabIndex = 2;
            this.button_searchall.Text = "搜索全部";
            this.button_searchall.UseVisualStyleBackColor = true;
            this.button_searchall.Click += new System.EventHandler(this.button_searchall_Click);
            // 
            // state
            // 
            this.state.Text = "状态";
            this.state.Width = 70;
            // 
            // Form_addFriend
            // 
            this.AcceptButton = this.button_search;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 483);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form_addFriend";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "添加朋友";
            this.Load += new System.EventHandler(this.Form_addFriend_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_search;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView listView_searchFriend;
        private System.Windows.Forms.Button button_search;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_searchall;
        private System.Windows.Forms.Label label_state;
        private System.Windows.Forms.ColumnHeader account;
        private System.Windows.Forms.ColumnHeader username;
        private System.Windows.Forms.ColumnHeader sex;
        private System.Windows.Forms.Button button_add;
        private System.Windows.Forms.ColumnHeader state;
    }
}