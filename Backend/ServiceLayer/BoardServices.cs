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
    internal class BoardServices
    {
        private UserFacade uFacade;
        private BoardFacade bFacade;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BoardServices(UserFacade uFacade, BoardFacade bFacade)
        {
            this.uFacade = uFacade;
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
                this.uFacade.CreateBoard(boardName.ToLower(), email); // creates the board
                bFacade.RegisterInFacade(uFacade.GetUserBL(email)); // adds the list in the board facade if user isnt registerd
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
                this.bFacade.DeleteBoard(email, boardName.ToLower());
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
                this.bFacade.Setlimit(email, boardName.ToLower(), col, limit);
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
                ColumnBL c = this.bFacade.GetColByID(email, bName.ToLower(), col);
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
                ColumnBL c = this.bFacade.GetColByID(email, bName.ToLower(), col);
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
                ColumnBL c = this.bFacade.GetColByID(email, bName.ToLower(), col);
                res = new Response<List<TaskSL>>(null,ConvertTaskList(c.Tasks)); //converts the collumn taskList to list of TaskListSL
            }
            catch (Exception ex)
            {
                res = new Response<List<TaskSL>>(ex.Message,null);
                Log.Error("Error in finding column tasks - BOARDSERVICES:" + ex.Message);
            }
            return JsonSerializer.Serialize(res);
        }
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

        public string JoinBoard(string email,string bName,string newUser)
        {
            throw new NotImplementedException();
        }

        public string LeaveBoard(string email, string bName, string newUser)
        {
            throw new NotImplementedException();
        }

        public string ChangeOwner(string email, string bName, string newUser)
        {
            throw new NotImplementedException();
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



    }
}
