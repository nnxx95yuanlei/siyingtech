using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form_serverInfo : Form
    {
        public Form_serverInfo()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.DialogResult = DialogResult.OK;
        }

        private void Form_serverInfo_Load(object sender, EventArgs e)
        {
            textBox1.Select(0, 0);
        }
    }
}
