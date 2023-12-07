using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckPCBSystem
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            this.textBoxName.Text = "admin";
            this.textBoxPassword.Text = "123456";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var name = textBoxName.Text.Trim();
            var password = textBoxPassword.Text.Trim();
            if(name == "admin" && password == "123456")
            {
                // 打开MainWindow
                var main = new MainWindow();
                main.Show();
                this.Hide();
            }
        }

        private void linkLabelRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // 打开注册窗口
        }
    }
}
