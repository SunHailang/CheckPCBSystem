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
    public partial class RegisterWindow : Form
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string name = this.textBoxUser.Text.Trim();
            byte[] nameBytes = Encoding.UTF8.GetBytes(name);
            if (nameBytes.Length < 5 || nameBytes.Length >= 20)
            {
                MessageBox.Show("用户名不规范，长度在5至20个字符。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string password = this.textBoxPassword.Text.Trim();
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            if (passwordBytes.Length < 6 || passwordBytes.Length >= 20)
            {
                MessageBox.Show("密码不规范，长度在6至20个字符。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MySqlConnect mySql = new MySqlConnect(MySqlData.SqlData);
            bool isexist = mySql.IsExist(name);
            if (isexist)
            {
                MessageBox.Show("用户名已存在！！！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string ret = mySql.RegisterUser(name, password);
            mySql.Dispose();
            if (string.IsNullOrEmpty(ret))
            {
                this.Close();
            }
            else
            {
                MessageBox.Show($"注册失败，请重新尝试！Error:{ret}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBoxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            bool isShow = this.checkBoxShowPassword.Checked;
            this.textBoxPassword.PasswordChar = !isShow ? '*' : '\0';
        }

        private void RegisterWindow_Load(object sender, EventArgs e)
        {
            this.textBoxPassword.PasswordChar = '*';
            this.checkBoxShowPassword.Checked = false;
        }
    }
}
