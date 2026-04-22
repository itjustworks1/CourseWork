using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Разработка_магазина_для_продажи_стройматериалов.Model;

namespace Разработка_магазина_для_продажи_стройматериалов.DB
{
    public class OrderDB
    {
        DBConnection connection;
        private OrderDB(DBConnection db)
        {
            this.connection = db;
        }
        public List<Order> SearchEquipment(string search)
        {
            List<Order> orders = new();

            string Ы = $"SELECT * FROM `Order` o";
            if (connection.OpenConnection())
            {
                using (var mc = connection.CreateCommand(Ы))
                {
                    mc.Parameters.Add(new MySqlParameter("search", $"%{search}%"));
                    using (var dr = mc.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var order = new Order();
                            order.Id = dr.GetInt32("Id");
                            order.Date = dr.GetDateTime("Date");
                            order.Status = dr.GetBoolean("Status");
                            orders.Add(order);

                        }
                    }
                    connection.CloseConnection();
                }
            }
            return orders;
        }
        public bool Insert(Order order)
        {
            bool result = false;
            if (connection == null)
                return result;
            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Order` Values(0,@Date,@Status);");
                cmd.Parameters.Add(new MySqlParameter("Date", order.Date));
                cmd.Parameters.Add(new MySqlParameter("Status", order.Status));
                try
                {
                    cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }
        internal List<Order> SelectAll()
        {
            List<Order> result = new List<Order>();
            if (connection == null) return result; if (connection.OpenConnection())
            {
                var cmd = connection.CreateCommand("SELECT * FROM `Order` o");
                try
                {
                    MySqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        DateTime date = new DateTime();
                        if (!dr.IsDBNull(1))
                            date = dr.GetDateTime("Date");
                        bool status = dr.GetBoolean("Status");
                        result.Add(new Order()
                        {
                            Id = id,
                            Date = date,
                            Status = status
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }
        internal bool Update(Order edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var cmd = connection.CreateCommand($"update `Order` set `Date`=@Date, `Status`=@Status  where `Id` = {edit.Id}");
                cmd.Parameters.Add(new MySqlParameter("Date", edit.Date));
                cmd.Parameters.Add(new MySqlParameter("Status", edit.Status));

                try
                {
                    cmd.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }
        static OrderDB db;
        public static OrderDB GetDB()
        {
            if (db == null)
                db = new OrderDB(DBConnection.GetDbConnection());
            return db;
        }

        internal bool Remove(Order selectedProduct)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `Order` where `id` = {selectedProduct.Id}");
                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }
    }
}
