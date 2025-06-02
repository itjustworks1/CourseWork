using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Разработка_магазина_для_продажи_стройматериалов.Model;

namespace Разработка_магазина_для_продажи_стройматериалов.DB
{
    public class OrderStructureDB
    {
        DBConnection connection;
        private OrderStructureDB(DBConnection db)
        {
            this.connection = db;
        }
        public List<OrderStructure> SearchOrderStructure(string search)
        {
            List<OrderStructure> orderStructures = new();
            List<Product> products = new();
            List<Order> orders = new();

            string Ы = $"SELECT * FROM OrderStructure os JOIN Product p ON os.ProductId = p.Id JOIN `Order` o ON os.OrderId = o.Id";
            if (connection.OpenConnection())
            {
                using (var mc = connection.CreateCommand(Ы))
                {
                    mc.Parameters.Add(new MySqlParameter("search", $"%{search}%"));
                    using (var dr = mc.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var orderStructure = new OrderStructure();
                            orderStructure.Id = dr.GetInt32("Id");
                            orderStructure.Quantity = dr.GetInt32("Quantity");
                            orderStructure.Value = dr.GetDecimal("Value");
                            orderStructure.ProductId = dr.GetInt32("ProductId");
                            orderStructure.OrderId = dr.GetInt32("OrderId");

                            var product = products.FirstOrDefault(d => d.Id == orderStructure.ProductId);
                            if (product == null)
                            {
                                product = new Product();
                                product.Id = orderStructure.ProductId;
                                product.Title = dr.GetString("Title");
                                product.Value = dr.GetDecimal("Value");
                                product.Quantity = dr.GetInt32("Quantity");
                                product.ProductTypeId = dr.GetInt32("ProductTypeId");
                                products.Add(product);
                            }

                            var order = orders.FirstOrDefault(d => d.Id == orderStructure.OrderId);
                            if (order == null)
                            {
                                order = new Order();
                                order.Id = orderStructure.OrderId;
                                order.Date = dr.GetDateTime("Date");
                                order.Status = dr.GetBoolean("Status");
                                orders.Add(order);
                            }

                            orderStructure.Product = product;
                            orderStructure.Order = order;
                            orderStructures.Add(orderStructure);

                        }
                    }
                    connection.CloseConnection();
                }
            }
            return orderStructures;
        }
        public bool Insert(OrderStructure orderStructure)
        {
            bool result = false;
            if (connection == null)
                return result;
            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `OrderStructure` Values(0,@Quantity,@Value,@ProductId,@OrderId);");
                cmd.Parameters.Add(new MySqlParameter("Quantity", orderStructure.Quantity));
                cmd.Parameters.Add(new MySqlParameter("Value", orderStructure.Value));
                cmd.Parameters.Add(new MySqlParameter("ProductId", orderStructure.ProductId));
                cmd.Parameters.Add(new MySqlParameter("OrderId", orderStructure.OrderId));
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
        internal List<OrderStructure> SelectAll()
        {
            List<OrderStructure> result = new List<OrderStructure>();
            if (connection == null) return result; if (connection.OpenConnection())
            {
                var cmd = connection.CreateCommand("SELECT os.`Id` AS osid, os.`Quantity` AS osquantity, os.`Value` AS osvalue, os.`ProductId`, os.`OrderId`, p.`Title`, p.`Value` AS pvalue, p.`Quantity` AS pquantity, p.`ProductTypeId`, o.`Date`, o.`Status` FROM OrderStructure os JOIN Product p ON os.ProductId = p.Id JOIN `Order` o ON os.OrderId = o.Id");
                try
                {
                    MySqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        int osquantity = 0;
                        if (!dr.IsDBNull(1))
                            osquantity = dr.GetInt32("osquantity");
                        decimal osvalue = 0;
                        if (!dr.IsDBNull(2))
                            osvalue = dr.GetDecimal("osvalue");
                        int ProductId = dr.GetInt16("ProductId");
                        int OrderId = dr.GetInt16("OrderId");

                        string Title = string.Empty;
                        if (!dr.IsDBNull(3))
                            Title = dr.GetString("Title");
                        decimal pvalue = 0;
                        if (!dr.IsDBNull(4))
                            pvalue = dr.GetDecimal("pvalue");
                        int pquantity = 0;
                        if (!dr.IsDBNull(5))
                            pquantity = dr.GetInt32("pquantity");
                        int ProductTypeId = dr.GetInt16("ProductTypeId");

                        DateTime Date = new DateTime();
                        if (!dr.IsDBNull(7))
                            Date = dr.GetDateTime("Date");
                        bool Status = dr.GetBoolean("Status");


                        Product product = new Product()
                        {
                            Id = ProductId,
                            Title = Title,
                            Value = pvalue,
                            Quantity = pquantity,
                            ProductTypeId = ProductTypeId
                        };

                        Order order = new Order()
                        {
                            Id = OrderId,
                            Date = Date,
                            Status = Status
                        };

                        result.Add(new OrderStructure()
                        {
                            Id = id,
                            Quantity = osquantity,
                            Value = osvalue,
                            Product = product,
                            ProductId = ProductId,
                            Order = order,
                            OrderId = OrderId
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
        static OrderStructureDB db;
        public static OrderStructureDB GetDB()
        {
            if (db == null)
                db = new OrderStructureDB(DBConnection.GetDbConnection());
            return db;
        }

        internal bool Update(OrderStructure edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var cmd = connection.CreateCommand($"update `OrderStructure` set `Quantity`=@Quantity, `Value`=@Value, `ProductId`=@ProductId, `OrderId`=@OrderId  where `Id` = {edit.Id}");
                cmd.Parameters.Add(new MySqlParameter("Quantity", edit.Quantity));
                cmd.Parameters.Add(new MySqlParameter("Value", edit.Value));
                cmd.Parameters.Add(new MySqlParameter("ProductId", edit.ProductId));
                cmd.Parameters.Add(new MySqlParameter("OrderId", edit.OrderId));

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
        internal bool Remove(OrderStructure selectedOrderStructure)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `OrderStructure` where `id` = {selectedOrderStructure.Id}");
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
