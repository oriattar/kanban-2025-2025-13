using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

using System.IO;
using log4net;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class BoardController
    {
        private const string userBoardTableName = "BoardUser";
        private const string boardTableName = "Boards";


        private readonly string _connectionString;
        private readonly ColumnController columnController;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BoardController()
        {
            columnController = new ColumnController();
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
        }
        /// <summary>
        /// A method respobsible for loading all the boards from the database
        /// </summary>
        /// <returns></returns>
       public List<BoardDAL> SelectAll()
       {
            List<BoardDAL> res = new List<BoardDAL>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
               
                SQLiteCommand command = new SQLiteCommand(null, connection);
                SQLiteDataReader reader = null;
                try
                {
                    connection.Open();

                    command.CommandText = $"SELECT * from {boardTableName};";
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        res.Add(ConvertReaderToBoard(reader));
                    }

                }
                catch (Exception ex)
                {
                    Log.Error("Error in selecting all boards:" + ex);
                    throw;
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }

            foreach(BoardDAL b in res)
            {
                List<ColumnDAL> cols = this.LoadColumns(b.BoardID);
                b.SetColumns(cols[BoardDAL.BACKLOG], cols[BoardDAL.IN_PROGRESS], cols[BoardDAL.DONE]);

                b.ActiveUsers = this.LoadActiveUsers(b.BoardID);
            }

            Log.Info("All boards were loaded successfuly");
            return res;
        }

        /// <summary>
        /// Constructs a temporary boardDal and than we coninue to load his attributes from other tables
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        private BoardDAL ConvertReaderToBoard(SQLiteDataReader dataReader)
        {
            return new BoardDAL((int)dataReader.GetValue(0),dataReader.GetString(1),dataReader.GetString(2));
        }

        /// <summary>
        /// A method that loads the boards columns, using the column controller
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private List<ColumnDAL> LoadColumns(int id)
        {
            return this.columnController.SelectColumns(id);
        }

        /// <summary>
        /// A method that loads the list of active users for a board from the intersection databas.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private List<string> LoadActiveUsers(int id)
        {
            List<string> res = new List<string>();
            using (var connection = new SQLiteConnection(_connectionString))
            {

                SQLiteCommand command = new SQLiteCommand(null, connection);
                SQLiteDataReader reader = null;
                try
                {
                    connection.Open();

                    command.CommandText = $"SELECT email from {userBoardTableName} where idBoard ={id};";
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        res.Add(reader.GetString(0));
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
            return res;
        }

        /// <summary>
        /// A method responsible for inserting a board to the db.
        /// Inserting (id,name,owner) to boards
        /// Inserting each user from active user list, (user,boardID) to BoardUser
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
       public bool Insert(BoardDAL board)
       {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();

                    command.CommandText = $"INSERT INTO {boardTableName} ({BoardDAL.BoardIDColName},{BoardDAL.OwnerColName},{BoardDAL.NameColName}) " + "VALUES (@bIDVal,@bOwner,@bName);";
                    
                    SQLiteParameter bIDVal = new SQLiteParameter(@"bIDVal", board.BoardID);
                    SQLiteParameter bName = new SQLiteParameter(@"bName", board.BoardName);
                    SQLiteParameter bOwner = new SQLiteParameter(@"bOwner", board.BoardOwner);

                    command.Parameters.Clear();

                    command.Parameters.Add(bIDVal);
                    command.Parameters.Add(bName);
                    command.Parameters.Add(bOwner);

                    command.Prepare();

                    res = command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Log.Error("Error in inserting a board to db:" + ex);
                    throw;
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                this.InsertCols(board.Columns); // Inserting the baord columns
                return res > 0;
            }
        }


        /// <summary>
        /// Helper method that helps inserting the cols of the board
        /// </summary>
        /// <param name="cols"></param>
        private void InsertCols(Dictionary<int,ColumnDAL> cols)
        {
            this.columnController.Insert(cols[BoardDAL.BACKLOG]);
            this.columnController.Insert(cols[BoardDAL.IN_PROGRESS]);
            this.columnController.Insert(cols[BoardDAL.DONE]); // Inserting the baord columns
        }

        /// <summary>
        /// Helper method that calls for the removal of the input columns
        /// </summary>
        /// <param name="cols"></param>
        private void DeleteCols(Dictionary<int,ColumnDAL> cols)
        {
            this.columnController.Delete(cols[BoardDAL.BACKLOG]);
            this.columnController.Delete(cols[BoardDAL.IN_PROGRESS]);
            this.columnController.Delete(cols[BoardDAL.DONE]);
        }

        /// <summary>
        /// A method responsible for removing a board from the db, removing from bouth UserBoard and Boards table.
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public bool Delete(BoardDAL board)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();

                    command.CommandText = $"DELETE FROM {userBoardTableName} WHERE idBoard = @bIDVAL;";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@bIDVAL", board.BoardID);
                    command.Prepare();
                    res = command.ExecuteNonQuery();

                    command.CommandText = $"DELETE FROM {boardTableName} WHERE idBoard = @bIDVAL;";
                    command.Parameters.Clear(); 
                    command.Parameters.AddWithValue("@bIDVAL", board.BoardID);
                    command.Prepare();
                    res = command.ExecuteNonQuery();


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

                this.DeleteCols(board.Columns); //Deletes all cols

                return res > 0;
            }
        }

        /// <summary>
        /// A method responsible for updateing the owner field in the db
        /// </summary>
        /// <param name="owner"></param>
        /// <exception cref="NotImplementedException"></exception>
       public bool UpdateOwner(string owner,int bID)
       {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();

                    command.CommandText = $"update {boardTableName} set [{BoardDAL.OwnerColName}]=@bOwner where {BoardDAL.BoardIDColName}={bID}";
                    
                    SQLiteParameter bOwner = new SQLiteParameter(@"bOwner", owner);

                    command.Parameters.Clear();
                    command.Parameters.Add(bOwner);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Log.Error("Error with updating owner in the DB:" + ex);
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
        /// Adds a new user, boardID entry to the table BoardUser
        /// </summary>
        /// <param name="b"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool InsertUser(int bID,string user)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();

                    command.CommandText = $"INSERT INTO {userBoardTableName} ({BoardDAL.UserIDColName} , {BoardDAL.BoardIDColName}) " + "VALUES (@emailVal,@bIDVal);";
                    SQLiteParameter emailVal = new SQLiteParameter(@"emailVal", user);
                    SQLiteParameter bIDVal = new SQLiteParameter(@"bIDVal", bID);

                    command.Parameters.Clear();
                    command.Parameters.Add(emailVal);
                    command.Parameters.Add(bIDVal);

                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Log.Error("Error in inserting user-board entry to db:" +ex);
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
        /// A method responsible for deleting a (email,boardID) entry in the UserBoard db
        /// </summary>
        /// <param name="b"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool RemoveUser(BoardDAL b, string user)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();

                    command.CommandText = $"DELETE FROM {userBoardTableName} WHERE email = @emailVal and idBoard = @bIDVAL;";
                    SQLiteParameter emailVal = new SQLiteParameter(@"emailVal", user);
                    SQLiteParameter bIDVal = new SQLiteParameter(@"bIDVal", b.BoardID);

                    command.Parameters.Clear();
                    command.Parameters.Add(emailVal);
                    command.Parameters.Add(bIDVal);

                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Log.Error("Error in deleting user-board entry from db:" + ex);
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
