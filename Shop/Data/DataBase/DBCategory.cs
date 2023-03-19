using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Data.Interfaces;
using Shop.Data.Common;
using Shop.Data.Models;
using MySql.Data.MySqlClient;

namespace Shop.Data.DataBase
{
    public class DBCategory : ICategorys
    {
        public IEnumerable<Categorys> AllCategorys
        {
            get
            {
                List<Categorys> categorys = new List<Categorys>();
                MySqlConnection mySqlConnection = Connection.MySqlOpen();
                MySqlDataReader categorysData = Connection.MySqlQuery("SELECT * FROM mydb.categorys ORDER BY `Name`;", mySqlConnection);
                while (categorysData.Read())
                {
                    categorys.Add(new Categorys()
                    {
                        Id = categorysData.IsDBNull(0) ? -1 : categorysData.GetInt32(0),
                        Name = categorysData.IsDBNull(1) ? null : categorysData.GetString(1),
                        Description = categorysData.IsDBNull(2) ? null : categorysData.GetString(2)
                    });
                }
                mySqlConnection.Close();
                return categorys;
            }
        }
        public int Add(Categorys category)
        {
            MySqlConnection mySqlConnection = Connection.MySqlOpen();
            Connection.MySqlQuery($"INSERT INTO `categorys`(`Name`,`Description`) VALUES ('{category.Name}', '{category.Description}');", mySqlConnection);
            mySqlConnection.Close();
            int IdItem = -1;
            mySqlConnection = Connection.MySqlOpen();
            MySqlDataReader mySqlDataReaderItem = Connection.MySqlQuery(
                $"SELECT `id` FROM `categorys` WHERE `Name` = '{category.Name}' AND `Description` = '{category.Description}';", mySqlConnection);
            if (mySqlDataReaderItem.HasRows)
            {
                mySqlDataReaderItem.Read();
                IdItem = mySqlDataReaderItem.GetInt32(0);
            }
            mySqlConnection.Close();
            return IdItem;
        }

        public void Update(Categorys category, string deleteCategory)
        {
            MySqlConnection mySqlConnection = Connection.MySqlOpen();
            if (deleteCategory == "on")
            {
                Connection.MySqlQuery($"DELETE FROM `categorys` WHERE `id` = '{category.Id}'", mySqlConnection);
            }
            else
            {
                Connection.MySqlQuery($"UPDATE `categorys` SET `Name` = '{category.Name}' ,`Description` = '{category.Description}'" +
                    $"WHERE `id` = '{category.Id}'", mySqlConnection);
            }
            mySqlConnection.Close();
        }
    }
}
