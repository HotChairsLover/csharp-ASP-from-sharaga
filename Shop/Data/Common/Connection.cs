using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Shop.Data.Common
{
    public class Connection
    {
        readonly static string ConnectionData = "server=localhost;port=3306;database=mydb;uid=root;pwd=root;";
        //readonly static string ConnectionData = "server=localhost;port=3309;database=mydb;uid=root;";

        public static MySqlConnection MySqlOpen()
        {
            MySqlConnection NewMySqlConnection = new MySqlConnection(ConnectionData);
            NewMySqlConnection.Open();
            return NewMySqlConnection;
        }

        public static MySqlDataReader MySqlQuery(string Query, MySqlConnection connection)
        {
            MySqlCommand NewMySqlCommand = new MySqlCommand(Query, connection);
            return NewMySqlCommand.ExecuteReader();
        }

        public static void MySqlClose(MySqlConnection connection)
        {
            connection.Close();
        }

    }
}
