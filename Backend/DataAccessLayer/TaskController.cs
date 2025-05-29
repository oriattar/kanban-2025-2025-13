using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class TaskController
    {

        public const int PARAM_0 = 0;
        public const int PARAM_1 = 1;
        public const int PARAM_2 = 2;
        public const int PARAM_3 = 3;
        public const int PARAM_4 = 4;
        public const int PARAM_5 = 5;
        public const int PARAM_6 = 6;

        private const string TaskTableName = "Tasks";

        private readonly string _connectionString;

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        



        /// <summary>
        /// Initializes a new instance of the TaskController class.
        /// Sets up the database connection string and table name.
        /// </summary>
        public TaskController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path};Version=3;";
            

            Log.Info($"TaskController initialized with database path: {path} - TASKCONTROLLER");
        }



        /// <summary>
        /// Inserts a new task into the Tasks table.
        /// </summary>
        /// <param name="task">TaskDAL object containing task data.</param>
        /// <returns>True if the insert succeeded, false otherwise.</returns>
        public bool Insert(TaskDAL task)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    Log.Info($"Attempting to insert task with ID: {task.TaskId} - TASKCONTROLLER");
                    connection.Open();
                    command.CommandText = $@"
                INSERT INTO {TaskTableName} (
                    idTask, idColumn, creationTime, dueTime, title, description, asignee
                ) VALUES (
                    @taskId, @columnId, @creationTime, @dueTime, @title, @description, @asignee
                );";

                    command.Parameters.AddWithValue("@taskId", task.TaskId);
                    command.Parameters.AddWithValue("@columnId", task.ColID);
                    command.Parameters.AddWithValue("@creationTime", task.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@dueTime", task.DueAt.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@title", task.TaskTitle);
                    command.Parameters.AddWithValue("@description", task.TaskDescription);
                    command.Parameters.AddWithValue("@asignee", task.AssignedUser);

                    command.Prepare();
                    res = command.ExecuteNonQuery();
                    Log.Info($"Insert operation affected {res} rows.- TASKCONTROLLER");
                }
                catch (Exception e)
                {
                    Log.Error($"Failed to insert task with ID: {task.TaskId} - TASKCONTROLLER", e);
                    throw;
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                    Log.Info("Database connection closed.- TASKCONTROLLER");
                }
                return res > 0;
            }
        }

        /// <summary>
        /// Retrieves all tasks from the Tasks table.
        /// </summary>
        /// <returns>List of TaskDAL objects representing all tasks.</returns>
        public List<TaskDAL> SelectTasks(string colID)
        {
            List<TaskDAL> results = new List<TaskDAL>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                SQLiteDataReader reader = null;
                try
                {
                    connection.Open();
                    command.CommandText = $"SELECT * FROM {TaskTableName} WHERE idColumn = @colID;";
                    command.Parameters.AddWithValue("@colID", colID);

                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        results.Add(ConvertReaderToTask(reader));
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"Failed to select tasks for ColumnID: {colID} - TASKCONTROLLER", e);
                    throw;
                }
                finally
                {
                    reader?.Close();
                    command.Dispose();
                    connection.Close();
                }
            }
            return results;
        }


        /// <summary>
        /// Converts a SQLiteDataReader row to a TaskDAL object.
        /// </summary>
        /// <param name="reader">SQLiteDataReader positioned at a task row.</param>
        /// <returns>TaskDAL object with values from the current row.</returns>
        public TaskDAL ConvertReaderToTask(SQLiteDataReader reader)
        {
            Log.Info("Converting SQLiteDataReader row to TaskDAL object.- TASKCONTROLLER");
            try
            {
                var task = new TaskDAL(
                    reader.GetInt32(PARAM_0),    // TaskId
                    reader.GetString(PARAM_1),   // ColumnId
                    DateTime.ParseExact(reader.GetString(PARAM_2), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                    DateTime.ParseExact(reader.GetString(PARAM_3), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                    reader.GetString(PARAM_4),   // TaskTitle
                    reader.GetString(PARAM_5),   // TaskDescription
                    reader.GetString(PARAM_6)    // AssignedUser
                );

                Log.Info($"Converted task: {task.TaskId} - TASKCONTROLLER");
                return task;
            }
            catch (Exception ex)
            {
                Log.Error("Error converting SQLiteDataReader to TaskDAL.- TASKCONTROLLER", ex);
                throw;
            }
        }

        /// <summary>
        /// updates the input attribute to the input value in the db, for the input task.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        public bool Update(int id, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"UPDATE {TaskTableName} SET [{attributeName}] = @attributeValue WHERE idTask = @id"
                };

                try
                {
                    command.Parameters.AddWithValue("@attributeValue", attributeValue);
                    command.Parameters.AddWithValue("@id", id);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Log.Error($"Failed to update {attributeName} for ID {id}", ex);
                    throw;
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }

    }
}