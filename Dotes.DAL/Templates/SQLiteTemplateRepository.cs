using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Configuration;
using Dotes.BE.Entities;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;

namespace Dotes.DAL.Templates
{
    public class SQLiteTemplateRepository : ITemplateRepository
    {
        private string connectionString;

        public SQLiteTemplateRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:SQLiteConnection"];
        }

        internal IDbConnection Connection => new SqliteConnection(connectionString);

        public bool CreateTemplate(Template template)
        {
            try
            {
                using (var connection = (SqliteConnection)Connection)
                {
                    connection.Open();
                    var cmd = new SqliteCommand
                    {
                        Connection = connection,
                        CommandText = $"INSERT INTO " +
                        $"templates(uid, name, filename, createddate, updateddate, typeid, file, tags) " +
                        $"VALUES('{Guid.NewGuid()}', :name, :filename, '{DateTime.Now}', '{DateTime.Now}', :typeid, :file, :tags);",
                    };
                    cmd.Parameters.Add(new SqliteParameter("name", template.Name ?? ""));
                    cmd.Parameters.Add(new SqliteParameter("filename", template.FileName ?? ""));
                    cmd.Parameters.Add(new SqliteParameter("typeid", template.Type.Id));
                    cmd.Parameters.Add(new SqliteParameter("file", template.File));
                    cmd.Parameters.Add(new SqliteParameter("tags", JsonConvert.SerializeObject(template.Tags)));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                    return true;
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public Template GetTemplateById(long id)
        {
            Template result = null;
            try
            {
                using (var connection = (SqliteConnection)Connection)
                {
                    connection.Open();
                    var cmd = new SqliteCommand
                    {
                        Connection = connection,
                        CommandText =
                        "SELECT t.id, t.uid, t.name, t.filename, t.createddate, t.updateddate, t.tags, t.file, t.typeid, tt.name as typename FROM templates t " +
                        "LEFT JOIN template_types tt on t.typeid = tt.id " +
                        "WHERE isdeleted = 0 AND t.id = :id;",
                    };

                    cmd.Parameters.Add(new SqliteParameter("id", id));

                    using (var localReader = cmd.ExecuteReader())
                    {
                        while (localReader.Read())
                        {
                            result = GetTemplateWithType(localReader);
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public Template GetTemplateByUid(Guid uid)
        {
            Template result = null;
            try
            {
                using (var connection = (SqliteConnection)Connection)
                {
                    connection.Open();
                    var cmd = new SqliteCommand
                    {
                        Connection = connection,
                        CommandText =
                        "SELECT t.id, t.uid, t.name, t.filename, t.createddate, t.updateddate, t.tags, t.file, t.typeid, tt.name as typename FROM templates t " +
                        "LEFT JOIN template_types tt on t.typeid = tt.id " +
                        "WHERE isdeleted = 0 AND uid = :uid;",
                    };

                    cmd.Parameters.Add(new SqliteParameter("uid", uid.ToString()));

                    using (var localReader = cmd.ExecuteReader())
                    {
                        while (localReader.Read())
                        {
                            result = GetTemplateWithType(localReader);
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public List<Template> GetTemplates(int? typeId = null)
        {
            var result = new List<Template>();

            try
            {
                using (var connection = (SqliteConnection)Connection)
                {
                    connection.Open();
                    var cmd = new SqliteCommand
                    {
                        Connection = connection,
                        CommandText = 
                        "SELECT t.id, t.uid, t.name, t.filename, t.createddate, t.updateddate, t.tags, t.typeid, tt.name as typename FROM templates t " +
                        "LEFT JOIN template_types tt on t.typeid = tt.id " +
                        "WHERE isdeleted = 0",
                    };

                    if (typeId != null)
                    {
                        cmd.CommandText += " AND typeid = :typeid;";
                        cmd.Parameters.Add(new SqliteParameter("typeid", typeId ?? (object)DBNull.Value));
                    }

                    using (var localReader = cmd.ExecuteReader())
                    {
                        while (localReader.Read())
                        {
                            result.Add(GetTemplateWithType(localReader));
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return result;
            }
        }

        public bool UpdateTemplate(Template template)
        {
            try
            {
                using (var connection = (SqliteConnection)Connection)
                {
                    connection.Open();
                    var cmd = new SqliteCommand
                    {
                        Connection = connection,
                        CommandText = 
                        $"UPDATE templates set name = :name, filename = :filename, updateddate = '{DateTime.Now}', typeid = :typeid, file = :file, tags = :tags " +
                        $"WHERE id = :id",
                    };
                    cmd.Parameters.Add(new SqliteParameter("id", template.Id));
                    cmd.Parameters.Add(new SqliteParameter("name", template.Name ?? ""));
                    cmd.Parameters.Add(new SqliteParameter("filename", template.FileName ?? ""));
                    cmd.Parameters.Add(new SqliteParameter("typeid", template.Type.Id));
                    cmd.Parameters.Add(new SqliteParameter("file", template.File));
                    cmd.Parameters.Add(new SqliteParameter("tags", JsonConvert.SerializeObject(template.Tags)));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public bool SetTemplateAsDeleted(long id)
        {
            try
            {
                using (var connection = (SqliteConnection)Connection)
                {
                    connection.Open();
                    var cmd = new SqliteCommand
                    {
                        Connection = connection,
                        CommandText =
                        $"UPDATE templates set isdeleted = 1 WHERE id = :id",
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
                Console.WriteLine(ex);
                return false;
            }
        }

        #region private 

        private Template GetTemplateWithType(SqliteDataReader localReader)
        {
            if (localReader.GetField("uid") != null)
            {
                var template = new Template
                {
                    Uid = new Guid(localReader.GetField("uid").ToString()),
                    Id = (long)localReader.GetField("id"),
                    Name = (string)localReader.GetField("name"),
                    FileName = (string)localReader.GetField("filename"),
                    CreatedDate = DateTimeOffset.Parse(localReader.GetField("createddate").ToString()),
                    UpdatedDate = DateTimeOffset.Parse(localReader.GetField("updateddate").ToString()),
                    Tags = JsonConvert.DeserializeObject<List<Tag>>(localReader.GetField("tags").ToString().Replace("\"[", "[").Replace("]\"", "]").Replace("\\", ""))
                };

                var file = localReader.GetField("file");
                if (file != null)
                {
                    template.File = (byte[])file;
                }

                var typeId = localReader.GetField("typeid");
                if (typeId != null)
                {
                    template.Type = new TemplateType
                    {
                        Id = (long)localReader.GetField("typeid"),
                        Name = (string)localReader.GetField("typename")
                    };
                }
                else
                {
                    template.Type = new TemplateType();
                }

                return template;
            }

            return null;
        }

        #endregion
    }
}