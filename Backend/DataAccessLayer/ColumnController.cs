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

        private const string ColumnTableName = "Columns";
        private readonly string _connectionString;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly TaskController tController;

        public ColumnController()
        {
            this.tController = new TaskController();
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
        }
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
                    Log.Error("Error in reading users for a board:" + ex);
                    throw;
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }

            foreach(ColumnDAL col in res)
            {
                col.Tasks = this.tController.SelectTasks(col.ColID);
            }
            return res;
        }

        private ColumnDAL ConvertReaderToColumnDAL(SQLiteDataReader dataReader)
        {
            return new ColumnDAL(dataReader.GetString(0),(int)dataReader.GetValue(1),dataReader.GetString(2),(int)dataReader.GetValue(3));
        }

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

                }
                catch (Exception ex)
                {
                    Log.Error("Error in deleting board from db:" + ex);
                    throw;
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

                this.tController.DeleteTasks(col.ColID); //Deletes its tasks

                return res > 0;
            }
        }

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

                    SQLiteParameter cID = new SQLiteParameter(@"cID", column.ColID);
                    SQLiteParameter bID = new SQLiteParameter(@"bID", column.BoardID);
                    SQLiteParameter cName = new SQLiteParameter(@"cName", column.Name);
                    SQLiteParameter cLimit = new SQLiteParameter(@"cLimit", column.TaskLimit);

                    command.Parameters.Clear();

                    command.Parameters.Add(cID);
                    command.Parameters.Add(bID);
                    command.Parameters.Add(cName);
                    command.Parameters.Add(cLimit); // adding the parameters

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

                    command.CommandText = $"update {ColumnTableName} set [{ColumnDAL.LimitColumnName}]=@cLimit where {ColumnDAL.idColumnName}={column.ColID}";

                    SQLiteParameter cLimit = new SQLiteParameter(@"cLimit", limit);

                    command.Parameters.Clear();
                    command.Parameters.Add(cLimit);
                    command.Prepare();

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
