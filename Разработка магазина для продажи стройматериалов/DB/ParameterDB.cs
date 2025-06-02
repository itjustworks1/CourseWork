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
    public class ParameterDB
    {
        DBConnection connection;
        private ParameterDB(DBConnection db)
        {
            this.connection = db;
        }
        public List<Parameter> SearchEquipment(string search)
        {
            List<Parameter> parameters = new();

            string Ы = $"SELECT * FROM Parameter p";
            if (connection.OpenConnection())
            {
                using (var mc = connection.CreateCommand(Ы))
                {
                    mc.Parameters.Add(new MySqlParameter("search", $"%{search}%"));
                    using (var dr = mc.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var parameter = new Parameter();
                            parameter.Id = dr.GetInt32("Id");
                            parameter.Title = dr.GetString("Title");
                            parameters.Add(parameter);

                        }
                    }
                    connection.CloseConnection();
                }
            }
            return parameters;
        }
        public bool Insert(Parameter parameter)
        {
            bool result = false;
            if (connection == null)
                return result;
            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Parameter` Values(0,@Title);");
                cmd.Parameters.Add(new MySqlParameter("Title", parameter.Title));
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
        internal List<Parameter> SelectAll()
        {
            List<Parameter> result = new List<Parameter>();
            if (connection == null) return result; if (connection.OpenConnection())
            {
                var cmd = connection.CreateCommand("SELECT p.`Id`, p.`Title` FROM Parameter p");
                try
                {
                    MySqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string Title = string.Empty;
                        if (!dr.IsDBNull(1))
                            Title = dr.GetString("Title");

                        result.Add(new Parameter()
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
        internal bool Update(Parameter edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var cmd = connection.CreateCommand($"update `Product` set `Title`=@Title where `Id` = {edit.Id}");
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
        static ParameterDB db;
        public static ParameterDB GetDB()
        {
            if (db == null)
                db = new ParameterDB(DBConnection.GetDbConnection());
            return db;
        }
        internal bool Remove(Parameter selectedParameter)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var cmd = connection.CreateCommand($"delete from `Parameter` where `id` = {selectedParameter.Id}");
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
