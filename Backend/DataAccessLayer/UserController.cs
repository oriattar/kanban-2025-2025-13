using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class UserController
    {

        private const string UserTableName = "Users";

        private readonly string _connectionString;
        private readonly string _tableName;

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        /// <summary>
        /// Initializes a new instance of the UserController class.
        /// Sets up the database connection string and table name.
        /// </summary>
        public UserController()
        {

            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));

            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = UserTableName;

            Log.Info($"UserController initialized with database path: {path} - USERCONTROLLER");

        }



        /// <summary>
        /// Retrieves all users from the Users table.
        /// </summary>
        /// <returns>List of UserDAL objects representing all users.</returns>
        public List<UserDAL> SelectAll()
        {
            Log.Info("SelectAll called - retrieving all users.- USERCONTROLLER");

            List<UserDAL> results = new List<UserDAL>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    Log.Info("Database connection opened.- USERCONTROLLER");

                    dataReader = command.ExecuteReader();
                    Log.Info("Executed SELECT query on Users table.- USERCONTROLLER");

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToForum(dataReader));
                    }
                    Log.Info($"Retrieved {results.Count} users from database.- USERCONTROLLER");
                }

                catch (Exception e)
                {
                    Log.Error("Error occurred during SelectAll.- USERCONTROLLER", e);
                    throw;
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                        Log.Info("DataReader closed.- USERCONTROLLER");

                    }

                    command.Dispose();
                    connection.Close();
                    Log.Info("Database connection closed.- USERCONTROLLER");

                }

            }
            return results;
        }


        /// <summary>
        /// Converts a SQLiteDataReader row to a UserDAL object.
        /// </summary>
        /// <param name="reader">SQLiteDataReader positioned at a user row.</param>
        /// <returns>UserDAL object with values from the current row.</returns>
        public UserDAL ConvertReaderToForum(SQLiteDataReader reader)
        {
            Log.Info("Converting SQLiteDataReader row to UserDAL object.- USERCONTROLLER");
            try
            {
                var user = new UserDAL(reader.GetString(0), reader.GetString(1));
                Log.Info($"Converted user: {user.Email}- USERCONTROLLER");
                return user;
            }
            catch (Exception ex)
            {
                Log.Error("Error converting SQLiteDataReader to UserDAL.- USERCONTROLLER", ex);
                throw;
            }
        }





    }
}
