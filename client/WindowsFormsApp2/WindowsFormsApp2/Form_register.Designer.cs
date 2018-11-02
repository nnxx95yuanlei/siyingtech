namespace WindowsFormsApp2
{
    partial class Form_register
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_register));
            this.textBox_account = new System.Windows.Forms.TextBox();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.textBox_password2 = new System.Windows.Forms.TextBox();
            this.textBox_username = new System.Windows.Forms.TextBox();
            this.comboBox_sex = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label_sex = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label_username = new System.Windows.Forms.Label();
            this.label_password2 = new System.Windows.Forms.Label();
            this.label_password = new System.Windows.Forms.Label();
            this.label_account = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.button_checkAccount = new System.Windows.Forms.Button();
            this.button_register = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_account
            // 
            this.textBox_account.Location = new System.Drawing.Point(110, 69);
            this.textBox_account.Name = "textBox_account";
            this.textBox_account.Size = new System.Drawing.Size(139, 21);
            this.textBox_account.TabIndex = 0;
            this.textBox_account.Leave += new System.EventHandler(this.textBox_account_Leave);
            // 
            // textBox_password
            // 
            this.textBox_password.Location = new System.Drawing.Point(110, 123);
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.PasswordChar = '*';
            this.textBox_password.Size = new System.Drawing.Size(139, 21);
            this.textBox_password.TabIndex = 1;
            this.textBox_password.Leave += new System.EventHandler(this.textBox_password_Leave);
            // 
            // textBox_password2
            // 
            this.textBox_password2.Location = new System.Drawing.Point(110, 176);
            this.textBox_password2.Name = "textBox_password2";
            this.textBox_password2.PasswordChar = '*';
            this.textBox_password2.Size = new System.Drawing.Size(139, 21);
            this.textBox_password2.TabIndex = 2;
            this.textBox_password2.Leave += new System.EventHandler(this.textBox_password2_Leave);
            // 
            // textBox_username
            // 
            this.textBox_username.Location = new System.Drawing.Point(98, 205);
            this.textBox_username.Name = "textBox_username";
            this.textBox_username.Size = new System.Drawing.Size(139, 21);
            this.textBox_username.TabIndex = 0;
            this.textBox_username.Leave += new System.EventHandler(this.textBox_username_Leave);
            // 
            // comboBox_sex
            // 
            this.comboBox_sex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_sex.FormattingEnabled = true;
            this.comboBox_sex.Items.AddRange(new object[] {
            "男",
            "女",
            "人妖"});
            this.comboBox_sex.Location = new System.Drawing.Point(110, 281);
            this.comboBox_sex.Name = "comboBox_sex";
            this.comboBox_sex.Size = new System.Drawing.Size(139, 20);
            this.comboBox_sex.TabIndex = 3;
            this.comboBox_sex.Leave += new System.EventHandler(this.comboBox_sex_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "注册账号:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "密码：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 155);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "确认密码：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 208);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "用户名：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 259);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "性别：";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label_sex);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label_username);
            this.groupBox1.Controls.Add(this.label_password2);
            this.groupBox1.Controls.Add(this.label_password);
            this.groupBox1.Controls.Add(this.label_account);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.button_checkAccount);
            this.groupBox1.Controls.Add(this.button_register);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.textBox_username);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(413, 396);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "注册信息";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label_sex
            // 
            this.label_sex.AutoSize = true;
            this.label_sex.ForeColor = System.Drawing.Color.Red;
            this.label_sex.Location = new System.Drawing.Point(98, 236);
            this.label_sex.Name = "label_sex";
            this.label_sex.Size = new System.Drawing.Size(47, 12);
            this.label_sex.TabIndex = 27;
            this.label_sex.Text = "label17";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(244, 210);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(149, 12);
            this.label16.TabIndex = 26;
            this.label16.Text = "16个以内的任意字符或汉字";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(227, 126);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(137, 12);
            this.label15.TabIndex = 25;
            this.label15.Text = "符，不能包含中文或符号";
            // 
            // label_username
            // 
            this.label_username.AutoSize = true;
            this.label_username.ForeColor = System.Drawing.Color.Red;
            this.label_username.Location = new System.Drawing.Point(98, 187);
            this.label_username.Name = "label_username";
            this.label_username.Size = new System.Drawing.Size(47, 12);
            this.label_username.TabIndex = 24;
            this.label_username.Text = "label15";
            // 
            // label_password2
            // 
            this.label_password2.AutoSize = true;
            this.label_password2.ForeColor = System.Drawing.Color.Red;
            this.label_password2.Location = new System.Drawing.Point(100, 133);
            this.label_password2.Name = "label_password2";
            this.label_password2.Size = new System.Drawing.Size(47, 12);
            this.label_password2.TabIndex = 23;
            this.label_password2.Text = "label15";
            // 
            // label_password
            // 
            this.label_password.AutoSize = true;
            this.label_password.BackColor = System.Drawing.SystemColors.Control;
            this.label_password.ForeColor = System.Drawing.Color.Red;
            this.label_password.Location = new System.Drawing.Point(98, 80);
            this.label_password.Name = "label_password";
            this.label_password.Size = new System.Drawing.Size(47, 12);
            this.label_password.TabIndex = 22;
            this.label_password.Text = "label15";
            // 
            // label_account
            // 
            this.label_account.AutoSize = true;
            this.label_account.ForeColor = System.Drawing.Color.Red;
            this.label_account.Location = new System.Drawing.Point(98, 26);
            this.label_account.Name = "label_account";
            this.label_account.Size = new System.Drawing.Size(47, 12);
            this.label_account.TabIndex = 21;
            this.label_account.Text = "label15";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(249, 154);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(0, 12);
            this.label14.TabIndex = 20;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(247, 104);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(161, 12);
            this.label13.TabIndex = 19;
            this.label13.Text = "6到16位a-z,A-Z,0-9之间的字\r\n";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(225, 71);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(185, 12);
            this.label12.TabIndex = 18;
            this.label12.Text = "之间的字符，不能包含中文或符号";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(312, 47);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(95, 12);
            this.label11.TabIndex = 17;
            this.label11.Text = "输入a-z,A-Z,0-9";
            // 
            // button_checkAccount
            // 
            this.button_checkAccount.Location = new System.Drawing.Point(247, 41);
            this.button_checkAccount.Name = "button_checkAccount";
            this.button_checkAccount.Size = new System.Drawing.Size(61, 23);
            this.button_checkAccount.TabIndex = 2;
            this.button_checkAccount.Text = "检查账号";
            this.button_checkAccount.UseVisualStyleBackColor = true;
            this.button_checkAccount.Click += new System.EventHandler(this.button_checkAccount_Click);
            // 
            // button_register
            // 
            this.button_register.Location = new System.Drawing.Point(47, 323);
            this.button_register.Name = "button_register";
            this.button_register.Size = new System.Drawing.Size(163, 35);
            this.button_register.TabIndex = 1;
            this.button_register.Text = "立即注册";
            this.button_register.UseVisualStyleBackColor = true;
            this.button_register.Click += new System.EventHandler(this.button_register_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(16, 258);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(11, 12);
            this.label10.TabIndex = 14;
            this.label10.Text = "*";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(16, 208);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(11, 12);
            this.label9.TabIndex = 13;
            this.label9.Text = "*";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(16, 155);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(11, 12);
            this.label8.TabIndex = 12;
            this.label8.Text = "*";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(16, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(11, 12);
            this.label7.TabIndex = 11;
            this.label7.Text = "*";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(16, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(11, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "*";
            // 
            // Form_register
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 433);
            this.Controls.Add(this.comboBox_sex);
            this.Controls.Add(this.textBox_password2);
            this.Controls.Add(this.textBox_password);
            this.Controls.Add(this.textBox_account);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form_register";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "用户注册";
            this.Load += new System.EventHandler(this.Form_register_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_account;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.TextBox textBox_password2;
        private System.Windows.Forms.TextBox textBox_username;
        private System.Windows.Forms.ComboBox comboBox_sex;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button_checkAccount;
        private System.Windows.Forms.Button button_register;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label_username;
        private System.Windows.Forms.Label label_password2;
        private System.Windows.Forms.Label label_password;
        private System.Windows.Forms.Label label_account;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label_sex;
    }
}