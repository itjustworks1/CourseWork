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
    class ProductTypeParameterDB
    {
        DBConnection connection;
        private ProductTypeParameterDB(DBConnection db)
        {
            this.connection = db;
        }

        public bool Insert(ProductTypeParameter productTypeParameter)
        {
            bool result = false;
            if (connection == null)
                return result;
            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `ProductTypeParameter` Values(@ProductTypeId,@ParameterId);");
                cmd.Parameters.Add(new MySqlParameter("ProductTypeId", productTypeParameter.ProductTypeId));
                cmd.Parameters.Add(new MySqlParameter("ParameterId", productTypeParameter.ParameterId));
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
        internal List<ProductTypeParameter> SelectAll()
        {
            List<ProductTypeParameter> result = new List<ProductTypeParameter>();
            if (connection == null) return result; if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("SELECT ptp.`ProductTypeId`, ptp.`ParameterId`, pt.`Title` AS pttitle, p.`Title` AS ptitle FROM ProductTypeParameter ptp JOIN ProductType pt ON ptp.ProductTypeId = pt.Id JOIN Parameter p ON ptp.ParameterId = p.Id");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int ProductTypeId = dr.GetInt16("ProductTypeId");
                        int ParameterId = dr.GetInt16("ParameterId");
                        string pttitle = string.Empty;
                        if (!dr.IsDBNull(2))
                            pttitle = dr.GetString("pttitle");
                        string ptitle = string.Empty;
                        if (!dr.IsDBNull(3))
                            ptitle = dr.GetString("ptitle");

                        ProductType productType = new ProductType()
                        {
                            Id = ProductTypeId,
                            Title = pttitle,
                        };

                        Parameter parameter = new Parameter()
                        {
                            Id = ParameterId,
                            Title = ptitle,
                        };

                        result.Add(new ProductTypeParameter()
                        {
                            ProductType = productType,
                            ProductTypeId = ProductTypeId,
                            Parameter = parameter,
                            ParameterId = ParameterId
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
        static ProductTypeParameterDB db;
        public static ProductTypeParameterDB GetDB()
        {
            if (db == null)
                db = new ProductTypeParameterDB(DBConnection.GetDbConnection());
            return db;
        }

        internal bool UpdateParameter(ProductTypeParameter edit, Parameter parameter)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var cmd = connection.CreateCommand($"update `ProductTypeParameter` set `ProductTypeId`=@ProductTypeId, `ParameterId`=@ParameterId where `ProductTypeId` = {edit.ProductTypeId} AND `ParameterId` = {parameter.Id}");
                cmd.Parameters.Add(new MySqlParameter("ProductTypeId", edit.ProductTypeId));
                cmd.Parameters.Add(new MySqlParameter("ParameterId", edit.ParameterId));

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
        internal bool Remove(ProductTypeParameter selectedProductTypeParameter)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `ProductTypeParameter` where `ProductTypeId` = {selectedProductTypeParameter.ProductTypeId} AND `ParameterId` = {selectedProductTypeParameter.ParameterId}");
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
