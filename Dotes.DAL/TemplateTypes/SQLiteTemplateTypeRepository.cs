using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Dotes.BE.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Dotes.DAL.TemplateTypes
{
    public class SQLiteTemplateTypeRepository : ITemplateTypeRepository
    {
        private string connectionString;

        public SQLiteTemplateTypeRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:SQLiteConnection"];
        }

        internal IDbConnection Connection => new SqliteConnection(connectionString);


        public bool CreateTemplateType(TemplateType type)
        {
            try
            {
                using (var connection = (SqliteConnection)Connection)
                {
                    connection.Open();
                    var cmd = new SqliteCommand
                    {
                        Connection = connection,
                        CommandText = "INSERT INTO template_types(name) VALUES(:name);",
                    };
                    cmd.Parameters.Add(new SqliteParameter("name", type.Name ?? string.Empty));

                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public List<TemplateType> GetTemplateTypes()
        {
            try
            {
                using (var connection = (SqliteConnection)Connection)
                {
                    connection.Open();
                    var result = connection.Query<TemplateType>("SELECT * FROM template_types;").ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public bool UpdateTemplateType(TemplateType type)
        {
            try
            {
                using (var connection = (SqliteConnection)Connection)
                {
                    connection.Open();
                    var cmd = new SqliteCommand
                    {
                        Connection = connection,
                        CommandText = "UPDATE template_types SET name = :name where id = :id;",
                    };
                    cmd.Parameters.Add(new SqliteParameter("name", type.Name ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("id", type.Id));

                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool DeleteTemplateType(long id)
        {
            try
            {
                using (var connection = (SqliteConnection)Connection)
                {
                    connection.Open();
                    var cmd = new SqliteCommand
                    {
                        Connection = connection,
                        CommandText = "DELETE FROM template_types WHERE id = :id; UPDATE templates SET typeid = null WHERE typeid = :id",
                    };
                    cmd.Parameters.Add(new SqliteParameter("id", id));

                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}