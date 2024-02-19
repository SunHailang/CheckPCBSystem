using CheckPCBSystem.UGUI;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CheckPCBSystem
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        // create table UserTable(id int auto_increment primary key, name varchar(20) not null, password varchar(20) not null);

        private void Login_Load(object sender, EventArgs e)
        {

            CheckForIllegalCrossThreadCalls = false;
            //获取UI线程同步上下文
            App.Instance.InitSyncContext(SynchronizationContext.Current);

            // 读取本地 已登录账号
            string localUser = "localuser.bytes";
            using(FileStream fs = new FileStream(localUser, FileMode.OpenOrCreate, FileAccess.Read))
            {
                int len = (int)fs.Length;
                byte[] bytes = new byte[len];
                fs.Read(bytes, 0, len);
            }

            this.textBoxName.Text = "admin";
            this.textBoxPassword.Text = "123456";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var name = textBoxName.Text.Trim();
            var password = textBoxPassword.Text.Trim();
            MySqlConnect mySql = new MySqlConnect(MySqlData.SqlData);
            bool ret = mySql.Login(name, password);
            if (ret)
            {
                this.Dispose();
                this.Close();
                // 打开MainWindow
                var main = new MainWindow();
                main.SetAdmin(name);
                FormManager.Instance.OpenForm(main);
            }
            else
            {
                MessageBox.Show("账户或密码错误！", "Error", MessageBoxButtons.OK);
            }
        }

        private void linkLabelRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // 打开注册窗口
            RegisterWindow register = new RegisterWindow();
            register.ShowDialog();
        }
    }
}
