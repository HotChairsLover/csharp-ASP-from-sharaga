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
    public class DBItems : IItems
    {
        public IEnumerable<Categorys> categorys = new DBCategory().AllCategorys;

        public IEnumerable<Items> AllItems
        {
            get
            {
                List<Items> items = new List<Items>();
                MySqlConnection mySqlConnection = Connection.MySqlOpen();
                MySqlDataReader itemsData = Connection.MySqlQuery("SELECT * FROM mydb.items ORDER BY `Price`;", mySqlConnection);
                while (itemsData.Read())
                {
                    items.Add(new Items()
                    {
                        Id = itemsData.IsDBNull(0) ? -1 : itemsData.GetInt32(0),
                        Name = itemsData.IsDBNull(1) ? "" : itemsData.GetString(1),
                        Description = itemsData.IsDBNull(2) ? "" : itemsData.GetString(2),
                        Img = itemsData.IsDBNull(3) ? "" : itemsData.GetString(3),
                        Price = itemsData.IsDBNull(4) ? -1 : itemsData.GetInt32(4),
                        Category = itemsData.IsDBNull(5) ? null : categorys.Where(x => x.Id == itemsData.GetInt32(5)).First()
                    });
                }
                mySqlConnection.Close();
                return items;
            }
        }

        public int Add(Items Item)
        {
            MySqlConnection mySqlConnection = Connection.MySqlOpen();
            Connection.MySqlQuery($"INSERT INTO `items`(`Name`,`Description`,`Img`,`Price`,`IdCategory`) " +
                $"VALUES ('{Item.Name}', '{Item.Description}', '{Item.Img}', '{Item.Price}', '{Item.Category.Id}');", 
                mySqlConnection);
            mySqlConnection.Close();
            int IdItem = -1;
            mySqlConnection = Connection.MySqlOpen();
            MySqlDataReader mySqlDataReaderItem = Connection.MySqlQuery(
                $"SELECT `id` FROM `items` WHERE `Name` = '{Item.Name}' AND `Description` = '{Item.Description}' " +
                $"AND `Price` = '{Item.Price}' AND `IdCategory` = '{Item.Category.Id}'; ", mySqlConnection);
            if (mySqlDataReaderItem.HasRows)
            {
                mySqlDataReaderItem.Read();
                IdItem = mySqlDataReaderItem.GetInt32(0);
            }
            mySqlConnection.Close();
            return IdItem;
        }
        public void Update(Items Item, string deleteItem)
        {
            MySqlConnection mySqlConnection = Connection.MySqlOpen();
            if (deleteItem == "on")
            {
                Connection.MySqlQuery($"DELETE FROM `items` WHERE `id` = '{Item.Id}'", mySqlConnection);
            }
            else if (Item.Img == "NaN")
            {
                Connection.MySqlQuery($"UPDATE `items` SET `Name` = '{Item.Name}' ,`Description` = '{Item.Description}' ,`Price` = '{Item.Price}' ,`IdCategory` = '{Item.Category.Id}' " +
                    $"WHERE `id` = '{Item.Id}'",mySqlConnection);
            }
            else
            {
                Connection.MySqlQuery($"UPDATE `items` SET `Name` = '{Item.Name}' ,`Description` = '{Item.Description}', `Img` = '{Item.Img}' ,`Price` = '{Item.Price}' ,`IdCategory` = '{Item.Category.Id}' " +
                    $"WHERE `id` = '{Item.Id}'", mySqlConnection);
            }
            mySqlConnection.Close();
            
        }
    }
}
