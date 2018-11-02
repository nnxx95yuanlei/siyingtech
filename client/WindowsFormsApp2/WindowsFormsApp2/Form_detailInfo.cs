using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form_detailInfo : Form
    {
        private string myAccount = "";
        private string myUsername = "";
        private string mySex = "";
        private string myState = "";

        public Form_detailInfo()
        {
            InitializeComponent();
        }

        public Form_detailInfo(string detail)
        {
            InitializeComponent();

            string[] detailArray = detail.Split(';');
            if (detailArray.Length < 4) return;

            myAccount = detailArray[0];
            myUsername = detailArray[1];
            mySex = detailArray[2];
            myState = detailArray[3];
        }

        private void Form_detailInfo_Load(object sender, EventArgs e)
        {
            this.Text = myAccount + "的详细信息";

            textBox_account.Text = myAccount;
            textBox_username.Text = myUsername;
            textBox_sex.Text = mySex;

            if (myState.Equals("online"))
            {
                label_state.Text = "在线";
                label_state.ForeColor = Color.Green;
            }
            else if (myState.Equals("offline"))
            {
                label_state.Text = "离线";
                label_state.ForeColor = Color.Red;
            } else
            {
                label_state.Text = "未知";
                label_state.ForeColor = Color.Red;
            }
        }
    }
}
