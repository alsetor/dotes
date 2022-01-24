using System;
using Microsoft.Data.Sqlite;

namespace Dotes.DAL
{
    public static class DataReaderExtensions
    {
        public static object GetField(this SqliteDataReader reader, string columnName)
        {
            try
            {
                if (reader[columnName] == DBNull.Value || reader[columnName] is DBNull) return null;
                return reader[columnName];
            }
            catch
            {
                return null;
            }
        }
    }
}
