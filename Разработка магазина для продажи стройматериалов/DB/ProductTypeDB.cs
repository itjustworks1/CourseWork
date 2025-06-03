using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Разработка_магазина_для_продажи_стройматериалов.Model;

namespace Разработка_магазина_для_продажи_стройматериалов.DB
{
    class ProductTypeDB
    {
        DBConnection connection;
        private ProductTypeDB(DBConnection db)
        {
            this.connection = db;
        }
        public bool Insert(ProductType productType)
        {
            bool result = false;
            if (connection == null)
                return result;
            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `ProductType` Values(0,@Title);");
                cmd.Parameters.Add(new MySqlParameter("Title", productType.Title));
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
        internal List<ProductType> SelectAll()
        {
            List<ProductType> result = new List<ProductType>();
            if (connection == null) return result; if (connection.OpenConnection())
            {
                var cmd = connection.CreateCommand("SELECT pt.`Id`, pt.`Title` FROM ProductType pt");
                try
                {
                    MySqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string Title = string.Empty;
                        if (!dr.IsDBNull(1))
                            Title = dr.GetString("Title");

                        result.Add(new ProductType()
                        {
                            Id = id,
                            Title = Title
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
        internal bool Update(ProductType edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var cmd = connection.CreateCommand($"update `ProductType` set `Title`=@Title where `Id` = {edit.Id}");
                cmd.Parameters.Add(new MySqlParameter("Title", edit.Title));

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
        static ProductTypeDB db;
        public static ProductTypeDB GetDB()
        {
            if (db == null)
                db = new ProductTypeDB(DBConnection.GetDbConnection());
            return db;
        }

        internal bool Remove(ProductType selectedProductType)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var cmd = connection.CreateCommand($"delete from `ProductType` where `id` = {selectedProductType.Id}");
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
    }
}
