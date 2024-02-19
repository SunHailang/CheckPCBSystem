using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CheckPCBSystem
{
    public class MySqlData
    {
        public string Host;
        public string User;
        public string Password;
        public string Database;

        public static MySqlData SqlData = new MySqlData
        {
            Host = "localhost",
            User = "root",
            Password = "123",
            Database = "YoloCheckRoad"
        };
    }

    public class MySqlConnect : IDisposable
    {
        private MySqlConnection connection;

        public MySqlConnect(MySqlData data)
        {
            MySqlConnectionStringBuilder sqlBuilder = new MySqlConnectionStringBuilder();
            sqlBuilder.UserID = data.User;
            sqlBuilder.Password = data.Password;
            sqlBuilder.Server = data.Host;
            sqlBuilder.Database = data.Database;
            connection = new MySqlConnection(sqlBuilder.ConnectionString);
            connection.Open();
        }

        public bool IsExist(string name)
        {
            bool ret = false;
            string query = $"select * from usertable where name=\'{name}\'";
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                reader = cmd.ExecuteReader();
                ret = reader.Read();
            }
            finally
            {
                if(reader != null)
                {
                    reader.Close();
                }
            }
            return ret;
        }

        public string RegisterUser(string username, string password)
        {
            string ret = string.Empty;
            string query = $"insert into usertable values(0,\'{username}\', \'{password}\');";
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                reader = cmd.ExecuteReader();
            }
            catch(Exception error)
            {
                ret = error.Message;
            }
            finally
            {
                if(reader != null)
                {
                    reader.Close();
                }
            }
            return ret;
        }

        public bool Login(string username, string password)
        {
            bool ret = false;
            string query = $"select * from usertable where name=\'{username}\' and password=\'{password}\'";
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                reader = cmd.ExecuteReader();
                ret = reader.Read();
            }
            finally
            {
                if(reader != null )
                {
                    reader.Close();
                }
            }
            return ret;
        }

        public void QueryUserData(string query)
        {

        }

        public void Dispose()
        {
            if(connection != null)
            {
                connection.Dispose();
            }
        }
    }
}
