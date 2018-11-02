using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form_main());

            Form_login login = new Form_login();
            Form_main form_main = new Form_main(login.GetLoginAccount(), login.GetSocket());

            login.setParent(form_main);
            login.ShowDialog();

            form_main.setLoginAccount(login.GetLoginAccount());

            if (login.DialogResult == DialogResult.OK)
            {
                login.Dispose();
                Application.Run(form_main);
            }
            else
            {
                login.Dispose();
                System.Environment.Exit(0);
                return;
            }
        }
    }
}
