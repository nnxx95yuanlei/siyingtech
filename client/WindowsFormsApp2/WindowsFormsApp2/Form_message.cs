using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp2
{

    public partial class Form_message : Form
    {
        public Form_message()
        {
            InitializeComponent();
        }

        public Form_message(string text)
        {
            InitializeComponent();
            label_text.Text = text;
        }

        private void Form_message_Load(object sender, EventArgs e)
        {

        }
    }


    public class MessageBoxEx
    {
        public static void Show(string text)
        {
            //new Form_message(text).ShowDialog();
            MessageBox.Show(text);
        }
    }
}
