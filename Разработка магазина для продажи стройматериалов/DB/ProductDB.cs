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
    public class ProductDB
    {
        DBConnection connection;
        private ProductDB(DBConnection db)
        {
            this.connection = db;
        }
        public List<Product> SearchProduct(string search)
        {
            List<Product> products = new();
            List<ProductType> productTypes = new();

            string Ы = $"SELECT * FROM Product p JOIN ProductType pt ON p.ProductTypeId = pt.Id";
            if (connection.OpenConnection())
            {
                using (var mc = connection.CreateCommand(Ы))
                {
                    mc.Parameters.Add(new MySqlParameter("search", $"%{search}%"));
                    using (var dr = mc.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var product = new Product();
                            product.Id = dr.GetInt32("Id");
                            product.Title = dr.GetString("Title");
                            product.Value = dr.GetDecimal("Value");
                            product.ProductTypeId = dr.GetInt32("ProductTypeId");

                            var productType = productTypes.FirstOrDefault(d => d.Id == product.ProductTypeId);
                            if (productType == null)
                            {
                                productType = new ProductType();
                                productType.Id = product.ProductTypeId;
                                productType.Title = dr.GetString("Title");
                                productTypes.Add(productType);
                            }
                            product.ProductType = productType;
                            products.Add(product);

                        }
                    }
                    connection.CloseConnection();
                }
            }
            return products;
        }
        public bool Insert(Product product)
        {
            bool result = false;
            if (connection == null)
                return result;
            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Product` Values(0,@Title,@Value,@Quantity,@ProductTypeId);");
                cmd.Parameters.Add(new MySqlParameter("Title", product.Title));
                cmd.Parameters.Add(new MySqlParameter("Value", product.Value));
                cmd.Parameters.Add(new MySqlParameter("Quantity", product.Quantity));
                cmd.Parameters.Add(new MySqlParameter("ProductTypeId", product.ProductTypeId));
                int a = 0;
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
        internal List<Product> SelectAll()
        {
            List<Product> result = new List<Product>();
            if (connection == null) return result; if (connection.OpenConnection())
            {
                var cmd = connection.CreateCommand("SELECT p.`Id`, p.`Title` AS ptitle, p.`Value`, p.`Quantity`, p.`ProductTypeId`, pt.`Title` AS pttitle FROM Product p JOIN ProductType pt ON p.ProductTypeId = pt.Id");
                try
                {
                    MySqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string ptitle = string.Empty;
                        if (!dr.IsDBNull(1))
                            ptitle = dr.GetString("ptitle");
                        decimal value = 0;
                        if (!dr.IsDBNull(2))
                            value = dr.GetDecimal("Value");
                        int quantity = 0;
                        if (!dr.IsDBNull(3))
                            quantity = dr.GetInt32("Quantity");

                        int productTypeId = dr.GetInt16("ProductTypeId");
                        string pttitle = string.Empty;
                        if (!dr.IsDBNull(5))
                            pttitle = dr.GetString("pttitle");


                        ProductType productType = new ProductType()
                        {
                            Id = productTypeId,
                            Title = pttitle
                        };

                        result.Add(new Product()
                        {
                            Id = id,
                            Title = ptitle,
                            Value = value,
                            Quantity = quantity,
                            ProductType = productType,
                            ProductTypeId = productTypeId
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

        internal bool Update(Product edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var cmd = connection.CreateCommand($"update `Product` set `Title`=@Title, `Value`=@Value, `Quantity`=@Quantity, `ProductTypeId`=@ProductTypeId where `Id` = {edit.Id}");
                cmd.Parameters.Add(new MySqlParameter("Title", edit.Title));
                cmd.Parameters.Add(new MySqlParameter("Value", edit.Value));
                cmd.Parameters.Add(new MySqlParameter("Quantity", edit.Quantity));
                cmd.Parameters.Add(new MySqlParameter("ProductTypeId", edit.ProductTypeId));

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
        internal bool Remove(Product selectedProduct)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var cmd = connection.CreateCommand($"delete from `Product` where `id` = {selectedProduct.Id}");
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
        static ProductDB db;
        public static ProductDB GetDB()
        {
            if (db == null)
                db = new ProductDB(DBConnection.GetDbConnection());
            return db;
        }
    }
}
