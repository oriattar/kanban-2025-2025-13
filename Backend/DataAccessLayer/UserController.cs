using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class UserController
    {

        private const string UserTableName = "Users";

        private readonly string _connectionString;
        private readonly string _tableName;
        public UserController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "database.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = UserTableName;
        }
        public List<UserDAL> SelectAll()
        {
            List<UserDAL> results = new List<UserDAL>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToForum(dataReader));
                    }
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }

            }
            return results;
        }

        public UserDAL ConvertReaderToForum(SQLiteDataReader reader)
        {
            return new UserDAL(reader.GetString(0), reader.GetString(1));
        }



    }
}
