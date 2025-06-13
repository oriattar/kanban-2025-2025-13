using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buiesness_layer;
using log4net;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class BoardServices
    {
        private BoardFacade bFacade;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BoardServices(BoardFacade bFacade)
        {
            this.bFacade = bFacade;
        }
        /// <summary>
        /// Creates a new board and returns response<BoardSL> if opeartion was succefull, else response will only contain error msg.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public string CreateBoard(string email, string boardName)
        {
            Response<string> res = null;
            try
            {
                bFacade.RegisterInFacade(email); // adds the list in the board facade if user isnt registerd
                bFacade.CreateBoard(boardName, email);
                res = new Response<string>(null, null);
            }
            catch (Exception ex)
            {
                res = new Response<string>(ex.Message,null);
                Log.Error("Error in creating board BOARDSERVICES:" + ex.Message);
            }
            return JsonSerializer.Serialize(res); //converts the response to json
        }

        /// <summary>
        /// A method that operates the board deletion operation
        /// returns a response accordingly.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <returns></returns>
        public string DeleteBoard(string email, string boardName)
        {
            Response<string> res = null;
            try
            {
                this.bFacade.DeleteBoard(email, boardName);
                res = new Response<string>(null,null);
            }
            catch (Exception ex)
            {
                res = new Response<string>(ex.Message, null);
                Log.Error("Error in deleting board BOARDSERVICES:" + ex.Message);
            }
            return JsonSerializer.Serialize(res);
        }
        /// <summary>
        /// A method the operates the limit set operation.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="limit"></param>
        /// <param name="collumn"></param>
        /// <returns></returns>
        public string SetLimit(string email, string boardName, int limit ,int col)
        {
            Response<string> res = null;
            try
            {
                this.bFacade.Setlimit(email, boardName, col, limit);
                res = new Response<string>(null, null);
            }
            catch (Exception ex)
            {
                res = new Response<string> (ex.Message, null);
                Log.Error("Error in setting column limit BOARDSERVICES:" + ex.Message);
            }
            return JsonSerializer.Serialize(res);
        }
        /// <summary>
        /// A method that support the return column limit service.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="bName"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public string GetColLimit(string email, string bName, int col)
        {
            try
            {
                ColumnBL c = this.bFacade.GetColByID(email, bName, col);
                Response<int> res = new Response<int>(null, c.TaskLimit);
                return JsonSerializer.Serialize(res);
            }
            catch (Exception ex)
            {
                Response<string> res = new Response<string>(ex.Message, null);
                Log.Error("Error in finding column limit - BOARDSERVICES:" + ex.Message);
                return JsonSerializer.Serialize(res);
            }
            
        }

        /// <summary>
        /// A method that supports the get column name featrue
        /// </summary>
        /// <param name="email"></param>
        /// <param name="bName"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public string GetColName(string email, string bName, int col)
        {
            Response<string> res = null;
            try
            {
                ColumnBL c = this.bFacade.GetColByID(email, bName, col);
                res = new Response<string>(null, c.Name);
            }
            catch (Exception ex)
            {
                res = new Response<string>(ex.Message,null);
                Log.Error("Error in finding column name - BOARDSERVICES:" + ex.Message);
            }
            return JsonSerializer.Serialize(res);
        }
        /// <summary>
        /// A method that returns the column tasks,given its ID
        /// returns as respone of list of TaskSL
        /// </summary>
        /// <param name="email"></param>
        /// <param name="bName"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public string GetColTasks(string email, string bName, int col)
        {
            Response<List<TaskSL>> res = null;
            try
            {
                ColumnBL c = this.bFacade.GetColByID(email, bName, col);
                res = new Response<List<TaskSL>>(null,ConvertTaskList(c.Tasks)); //converts the collumn taskList to list of TaskListSL
            }
            catch (Exception ex)
            {
                res = new Response<List<TaskSL>>(ex.Message,null);
                Log.Error("Error in finding column tasks - BOARDSERVICES:" + ex.Message);
            }
            return JsonSerializer.Serialize(res);
        }
        /// <summary>
        /// Operates the get in progress tasks service - using the board facade
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string GetInProgressList(string email)
        {
            Response<List<TaskSL>> res = null;
            try
            {
                res = new Response<List<TaskSL>>(null,ConvertTaskList(this.bFacade.GetInProgressList(email)));
            }
            catch (Exception ex)
            {
                res = new Response<List<TaskSL>>(ex.Message, null);
                Log.Error("Error in finding inProgress tasks - BOARDSERVICES:" + ex.Message);
            }
            return JsonSerializer.Serialize(res);
        }
        /// <summary>
        /// Operates the get user board id list operation - serivce
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string GetUserBoardIds(string email)
        {
            Response<List<int>> res = null;
            try
            {
                this.bFacade.RegisterInFacade(email);
                res = new Response<List<int>>(null, this.bFacade.CollectBoardIds(email));
            }
            catch (Exception ex)
            {
                res = new Response<List<int>>(ex.Message, null);
                Log.Error("Error in locating board IDs - BOARDSERVICES:" + ex.Message);
            }
            return JsonSerializer.Serialize(res);
        }

        /// <summary>
        /// Operates the join board operation
        /// </summary>
        /// <param name="email"></param>
        /// <param name="bName"></param>
        /// <param name="newUser"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string JoinBoard(string email, int boardID)
        {
            Response<string> res = null;
            try
            {
                this.bFacade.RegisterInFacade(email); // ensuring an entry exist in facade
                this.bFacade.AddUserAsMember(email, boardID);
                res = new Response<string>(null, null);
            }
            catch (Exception ex)
            {
                res = new Response<string>(ex.Message, null);
                Log.Error("Error in adding a memeber user to a board - BOARDSERVICES:" + ex.Message);
            }
            return JsonSerializer.Serialize(res);
        }

        /// <summary>
        /// A method that operates the board leave operation
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardID"></param>
        /// <returns></returns>
        public string LeaveBoard(string email, int boardID)
        {
            Response<string> res = null;
            try
            {
                this.bFacade.RemoveUserAsMember(email,boardID);
                res = new Response<string>(null, null);
            }
            catch (Exception ex)
            {
                res = new Response<string>(ex.Message, null);
                Log.Error("Error in removing a memeber user from a board - BOARDSERVICES:" + ex.Message);
            }
            return JsonSerializer.Serialize(res);
        }
        /// <summary>
        /// Operates the ownership transfer operation
        /// </summary>
        /// <param name="currentOwnerEmail"></param>
        /// <param name="newOwnerEmail"></param>
        /// <param name="boardName"></param>
        /// <returns></returns>
        public string ChangeOwner(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            Response<string> res = null;
            try
            {
                this.bFacade.TransferOwnership(currentOwnerEmail, newOwnerEmail, boardName);
                res = new Response<string>(null, null);
            }
            catch (Exception ex)
            {
                res = new Response<string>(ex.Message, null);
                Log.Error("Error in transfering ownership of a board - BOARDSERVICES:" + ex.Message);
            }
            return JsonSerializer.Serialize(res);
        }

        /// <summary>
        /// Operates the get board name by id service.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetBoardName(int id)
        {
            Response<string> res = null;
            try
            {
                res = new Response<string>(null, this.bFacade.GetBoardNameById(id));
            }
            catch (Exception ex)
            {
                res = new Response<string>(ex.Message, null);
                Log.Error("Error in locating a board name by ID - BOARDSERVICES:" + ex.Message);

            }
            return JsonSerializer.Serialize(res);
        }
        /// <summary>
        /// A method that returns a taskSL list from taskBL list
        /// </summary>
        /// <returns></returns>
        public static List<TaskSL> ConvertTaskList(List<TaskBL> t)
        {
            if (t == null)
                return null;

            List<TaskSL> result = new List<TaskSL>();
            foreach (TaskBL b in t)
            {
                result.Add(new TaskSL(b));
            }

            return result;
        }

        /// <summary>
        /// Services that returns a list of a user board SL objects
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string GetBoards(string email)
        {
            Response<List<BoardSL>> res = null;
            try
            {
                List<BoardSL> list = bFacade.GetBoards(email).Select(b => new BoardSL(b)).ToList();
                res = new Response<List<BoardSL>>(null, list);

            }
            catch (Exception e)
            {
                res = new Response<List<BoardSL>>(e.Message, null);
                Log.Error("Error in collecting boards for the user. - BOARDSERVICES:" + e.Message);
            }
            return JsonSerializer.Serialize(res);
        }

        /// <summary>
        /// Service for getting a board sl from backend, using its name
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <returns></returns>
        public string GetBoard(string email, string boardName)
        {
            Response<BoardSL> res = null;
            try
            {
                res = new Response<BoardSL>(null, new BoardSL(this.bFacade.GetBoard(email, boardName)));
            }
            catch (Exception e)
            {
                res = new Response<BoardSL>(e.Message, null);
            }
            return JsonSerializer.Serialize(res);
        }
        /// <summary>
        /// Helper method that loads the boards on startup
        /// </summary>
        public void LoadBoards()
        {
            this.bFacade.LoadAllBoards();
        }


    }
}
