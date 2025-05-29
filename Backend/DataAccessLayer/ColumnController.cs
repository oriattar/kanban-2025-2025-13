using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class ColumnController
    {
        private const string TaskTableName = "Tasks";
        private const string ColumnTableName = "Columns";
        private readonly string _connectionString;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const int PARAM_0 = 0;
        public const int PARAM_1 = 1;
        public const int PARAM_2 = 2;
        public const int PARAM_3 = 3;



        public ColumnController()
        {
           
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
        }

        /// <summary>
        /// Selects the column of a board from the database, given the boardID.
        /// </summary>
        /// <param name="boardID"></param>
        /// <returns></returns>
        public List<ColumnDAL> SelectColumns(int boardID)
        {
            List<ColumnDAL> res = new List<ColumnDAL>();
            using (var connection = new SQLiteConnection(_connectionString))
            {

                SQLiteCommand command = new SQLiteCommand(null, connection);
                SQLiteDataReader reader = null;
                try
                {
                    connection.Open();

                    command.CommandText = $"SELECT * from {ColumnTableName} where idBoard ={boardID};";
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        res.Add(this.ConvertReaderToColumnDAL(reader));
                    }

                }
                catch (Exception ex)
                {
                    Log.Error("Error in selecting columns:" + ex);
                    throw;
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }

            //loop that collets each of the columns task from the db
            foreach (ColumnDAL col in res)
            {
                col.Tasks = new TaskController().SelectTasks(col.ColID);
            }

            return res;
        }

        /// <summary>
        /// Helper method that convers and sql read to ColumnDAL object
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        private ColumnDAL ConvertReaderToColumnDAL(SQLiteDataReader dataReader)
        {
            return new ColumnDAL(dataReader.GetString(PARAM_0),dataReader.GetInt32(PARAM_1),dataReader.GetString(PARAM_2),dataReader.GetInt32(PARAM_3));//null we be replaced after connection is closed
        }

        /// <summary>
        /// Deletes a col from the data base
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public bool Delete(ColumnDAL col)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();

                    command.CommandText = $"DELETE FROM {ColumnTableName} WHERE {ColumnDAL.idColumnName} = @cID;";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@cID", col.ColID);
                    command.Prepare();
                    res = command.ExecuteNonQuery(); // Deletes cols from DB

                    command.CommandText = $"DELETE FROM {TaskTableName} WHERE {ColumnDAL.idColumnName} = @cID;";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@cID", col.ColID);
                    command.Prepare();
                    res = command.ExecuteNonQuery(); // tasks of the cols in the tasks table

                }
                catch (Exception ex)
                {
                    Log.Error("Error in deleting a columns from db:" + ex);
                    throw;
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

                 
                return res > 0;
            }
        }

        /// <summary>
        /// A method responsible of inserting a column to the db
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public bool Insert(ColumnDAL column)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();

                    command.CommandText = $"INSERT INTO {ColumnTableName} ({ColumnDAL.idColumnName},{ColumnDAL.BoardColumnName},{ColumnDAL.NameColumnName},{ColumnDAL.LimitColumnName}) " +
                        $"" + "VALUES (@cID,@bID,@cName,@cLimit);";

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@cID", column.ColID);
                    command.Parameters.AddWithValue("@bID", column.BoardID);
                    command.Parameters.AddWithValue("@cName", column.Name);
                    command.Parameters.AddWithValue("@cLimit", column.TaskLimit); // adds parameters

                    command.Prepare();

                    res = command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Log.Error("Error in inserting a column to db:" + ex);
                    throw;
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                
                return res > 0;
            }

        }

        /// <summary>
        /// Updates the limit of a column in the database
        /// </summary>
        /// <param name="column"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public bool UpdateLimit(ColumnDAL column, int limit)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();

                    command.CommandText = $"UPDATE {ColumnTableName} SET [{ColumnDAL.LimitColumnName}] = @cLimit WHERE {ColumnDAL.idColumnName} = @cID";

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@cLimit", limit);
                    command.Parameters.AddWithValue("@cID", column.ColID);

                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Log.Error("Error with updating limit in the DB:" + ex);
                    throw;
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }
    }
}
