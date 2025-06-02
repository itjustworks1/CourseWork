using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Разработка_магазина_для_продажи_стройматериалов.Model;

namespace Разработка_магазина_для_продажи_стройматериалов.DB
{
    public class ProductParameterDB
    {
        DBConnection connection;
        private ProductParameterDB(DBConnection db)
        {
            this.connection = db;
        }
        //public List<OrderStructure> SearchEquipment(string search)
        //{
        //    List<OrderStructure> orderStructures = new();
        //    List<Product> products = new();
        //    List<Order> orders = new();

        //    string Ы = $"SELECT * FROM OrderStructure os JOIN Product p ON os.ProductId = p.Id JOIN `Order` o ON os.OrderId = o.Id";
        //    if (connection.OpenConnection())
        //    {
        //        using (var mc = connection.CreateCommand(Ы))
        //        {
        //            mc.Parameters.Add(new MySqlParameter("search", $"%{search}%"));
        //            using (var dr = mc.ExecuteReader())
        //            {
        //                while (dr.Read())
        //                {
        //                    var orderStructure = new OrderStructure();
        //                    orderStructure.Id = dr.GetInt32("Id");
        //                    orderStructure.Quantity = dr.GetInt32("Quantity");
        //                    orderStructure.Value = dr.GetDecimal("Value");
        //                    orderStructure.ProductId = dr.GetInt32("ProductId");
        //                    orderStructure.OrderId = dr.GetInt32("OrderId");

        //                    var product = products.FirstOrDefault(d => d.Id == orderStructure.ProductId);
        //                    if (product == null)
        //                    {
        //                        product = new Product();
        //                        product.Id = orderStructure.ProductId;
        //                        product.Title = dr.GetString("Title");
        //                        product.Value = dr.GetDecimal("Value");
        //                        product.Quantity = dr.GetInt32("Quantity");
        //                        product.ProductTypeId = dr.GetInt32("ProductTypeId");
        //                        products.Add(product);
        //                    }

        //                    var order = orders.FirstOrDefault(d => d.Id == orderStructure.OrderId);
        //                    if (order == null)
        //                    {
        //                        order = new Order();
        //                        order.Id = orderStructure.OrderId;
        //                        order.Date = dr.GetDateTime("Date");
        //                        order.Status = dr.GetBoolean("Status");
        //                        orders.Add(order);
        //                    }

        //                    orderStructure.Product = product;
        //                    orderStructure.Order = order;
        //                    orderStructures.Add(orderStructure);

        //                }
        //            }
        //            connection.CloseConnection();
        //        }
        //    }
        //    return orderStructures;
        //}
        public bool Insert(ProductParameter productParameter)
        {
            bool result = false;
            if (connection == null)
                return result;
            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `ProductParameter` Values(0,@Meaning,@ParameterId,@ProductId);");
                cmd.Parameters.Add(new MySqlParameter("Meaning", productParameter.Meaning));
                cmd.Parameters.Add(new MySqlParameter("ParameterId", productParameter.ParameterId));
                cmd.Parameters.Add(new MySqlParameter("ProductId", productParameter.ProductId));
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
        internal List<ProductParameter> SelectAll()
        {
            List<ProductParameter> result = new List<ProductParameter>();
            if (connection == null) return result; if (connection.OpenConnection())
            {
                var cmd = connection.CreateCommand("SELECT pp.`Id` AS ppid, pp.`Meaning`, pp.`ParameterId`, pp.`ProductId`, p.`Title` AS ptitle, p1.`Title` AS p1title, p1.`Value`, p1.`Quantity`, p1.`ProductTypeId` FROM ProductParameter pp JOIN Parameter p ON pp.ParameterId = p.Id JOIN Product p1 ON pp.ProductId = p1.Id");
                try
                {
                    MySqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string Meaning = string.Empty;
                        if (!dr.IsDBNull(1))
                            Meaning = dr.GetString("Meaning");
                        int ParameterId = dr.GetInt16("ParameterId");
                        int ProductId = dr.GetInt16("ProductId");

                        string ptitle = string.Empty;
                        if (!dr.IsDBNull(4))
                            ptitle = dr.GetString("ptitle");

                        string p1title = string.Empty;
                        if (!dr.IsDBNull(5))
                            p1title = dr.GetString("p1title");
                        decimal Value = 0;
                        if (!dr.IsDBNull(6))
                            Value = dr.GetDecimal("Value");
                        int Quantity = 0;
                        if (!dr.IsDBNull(7))
                            Quantity = dr.GetInt32("Quantity");
                        int ProductTypeId = dr.GetInt16("ProductTypeId");

                        Parameter parameter = new Parameter()
                        {
                            Id = ParameterId,
                            Title = ptitle
                        };

                        Product product = new Product()
                        {
                            Id = ProductId,
                            Title = p1title,
                            Value = Value,
                            Quantity = Quantity,
                            ProductTypeId = ProductTypeId
                        };


                        result.Add(new ProductParameter()
                        {
                            Id = id,
                            Meaning = Meaning,
                            Parameter = parameter,
                            ParameterId = ParameterId,
                            Product = product,
                            ProductId = ProductId
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
        static ProductParameterDB db;
        public static ProductParameterDB GetDB()
        {
            if (db == null)
                db = new ProductParameterDB(DBConnection.GetDbConnection());
            return db;
        }

        internal bool Update(ProductParameter edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var cmd = connection.CreateCommand($"update `ProductParameter` set `Meaning`=@Meaning, `ParameterId`=@ParameterId, `ProductId`=@ProductId  where `Id` = {edit.Id}");
                cmd.Parameters.Add(new MySqlParameter("Meaning", edit.Meaning));
                cmd.Parameters.Add(new MySqlParameter("ParameterId", edit.ParameterId));
                cmd.Parameters.Add(new MySqlParameter("ProductId", edit.ProductId));

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
        internal bool Remove(ProductParameter selectedProductParameter)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `ProductParameter` where `id` = {selectedProductParameter.Id}");
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
