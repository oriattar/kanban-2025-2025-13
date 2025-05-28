using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class SystemController
    {
        private const string SystemTableName = "System";
        private const string BoardUserTableName = "BoardUser";
        private const string BoardTableName = "Boards";
        private const string ColumnTableName = "Columns";
        private const string UserTableName = "Users";
        private const string TaskTableName = "Tasks";



        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string _connectionString;
        public SystemController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
        }

        /// <summary>
        /// Loads the BoardIDGen from the db
        /// </summary>
        /// <returns></returns>
        public int LoadBoardIDGen()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                SQLiteDataReader reader = null;
                try
                {
                    connection.Open();

                    command.CommandText = $"SELECT BoardIDGenerator from {SystemTableName} where Facade=@facade;";

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@facade", "BoardFacade");

                    reader = command.ExecuteReader();
                    reader.Read();
                    res = (int)reader.GetInt32(0);
                    if (reader != null)
                    {
                        reader.Close();
                    }

                }
                catch (Exception ex)
                {
                    Log.Error("Error in loading board id generator from db:" + ex);
                    throw;
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res;
            }
        }
        /// <summary>
        /// Loads the generator of task IDS fron the db
        /// </summary>
        /// <returns></returns>
        public int LoadTaskIDGen()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                SQLiteDataReader reader = null;
                try
                {
                    connection.Open();

                    command.CommandText = $"SELECT TaskIDGenerator from {SystemTableName} where Facade=@facade;";

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@facade", "BoardFacade");

                    reader = command.ExecuteReader();
                    reader.Read();
                    res = (int)reader.GetInt32(0);
                    if(reader!=null)
                    {
                        reader.Close();
                    }

                }
                catch (Exception ex)
                {
                    Log.Error("Error in loading task id generator:" + ex);
                    throw;
                }
                finally
                {
                   
                    command.Dispose();
                    connection.Close();
                }
                return res;
            }
        }

        /// <summary>
        /// updates the id counters in the db
        /// </summary>
        /// <param name="boardGen"></param>
        /// <param name="taskGen"></param>
        /// <returns></returns>
        public bool UpdateGenerators(int boardGen,int taskGen)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();

                    command.CommandText = $"UPDATE {SystemTableName} SET [BoardIDGenerator] = @bGen,[TaskIDGenerator] =@cGen WHERE Facade = @facade;";

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@bGen",boardGen);
                    command.Parameters.AddWithValue("@cGen", taskGen);
                    command.Parameters.AddWithValue("@facade", "BoardFacade");

                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Log.Error("Error with updating generators in db:" + ex);
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
        /// A method responsible for clearing the data from the DB
        /// Inserting the defualt id generators in the db
        /// </summary>
        public bool ClearData()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();

                    command.CommandText = $"DELETE FROM {TaskTableName};" +
                        $"DELETE FROM {ColumnTableName};"+
                        $"DELETE FROM {BoardUserTableName};"+
                        $"DELETE FROM {UserTableName};"+
                        $"DELETE FROM {BoardTableName};"+
                        $"DELETE FROM {SystemTableName};"
                        +$"INSERT INTO {SystemTableName} (Facade,BoardIDGenerator,TaskIDGenerator) VALUES (@facade,@def_gen,@def_task_gen);";

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@facade", "BoardFacade");
                    command.Parameters.AddWithValue("@def_gen", 0);
                    command.Parameters.AddWithValue("@def_task_gen", 0);

                    res = command.ExecuteNonQuery(); //clears data and inserting defualt ID Generator cols in the system db table

                }
                catch (Exception ex)
                {
                    Log.Error("Error in clearing data from db:" + ex);
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
