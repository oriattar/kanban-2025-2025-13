using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using IntroSE.Kanban.Backend.ServiceLayer;
using log4net;

namespace IntroSE.Kanban.Backend.Buiesness_layer
{
    public class BoardFacade
    {
        private Dictionary<string, List<BoardBL>> boards;
        private AuthenticationFacade aFacade;

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// A constructor to the boardFacade, inits the dict
        /// </summary>
        /// <param name="aFacade"></param>
        public BoardFacade(AuthenticationFacade aFacade)
        {
            this.boards  = new Dictionary<string, List<BoardBL>>();
            this.aFacade = aFacade;
        }

        /// <summary>
        /// A method that deletes a target board from a user's boardList
        /// A deletion will occur if the user is logged in, registered in facade and has the target board
        /// </summary>
        /// <param name="email"></param>
        /// <param name="bName"></param>
        /// <exception cref="Exception"></exception>
        public void DeleteBoard(string email, string bName)
        {
            if(!this.aFacade.IsLoggedIn(email))
            {
                Log.Warn("Cant delete a board while not logged in. - BOARDFACADE");
                throw new Exception("Cant delete a board while not logged in.");
            }

            BoardBL toDelte = this.GetBoard(email, bName);

            this.boards[email].Remove(toDelte); //deletes the board from users boardlist
            Log.Info("A board was deleted to the user:" + email + " in BOARDFACADE");
        }
        /// <summary>
        /// Registers a user's boardlist if one dosnt exist for the user in the facade
        /// </summary>
        /// <param name="b"></param>
        /// <exception cref="Exception"></exception>
        public void RegisterInFacade(UserBL user)
        {
            if (!this.boards.ContainsKey(user.Email))
            {
                this.boards.Add(user.Email,user.Boards);
                Log.Info("A list was added to the user:" + user.Email + " in BOARDFACADE");
            }
        }

        

        public void advanceTasK(string idtask, string boardname)
        {
            throw new NotImplementedException();
        }

        public TaskBL CreateTask(DateTime deadlinem, string title, string description, string bname)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// A method that sets the limit of the input collumn to the input limit,
        /// if all the input parameters are valid and found, and if there less or equal number of tasks in collumn to the new limit.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="bName"></param>
        /// <param name="collumn"></param>
        /// <param name="limit"></param>
        /// <exception cref="Exception"></exception>
        public void Setlimit(string email, string bName, int col, int limit)
        {
            BoardBL board = this.GetBoard(email,bName);
            if(limit <-1 || col <0 || col >2) //allows removing the limit aswell
            {
                Log.Warn("Can access collumn or ileagal limit - BOARDFACADE");
                throw new Exception("Ileagal collumn number or limit");
            }

            board.setLimit(col,limit);
        }

        public TaskSL EditTask(DateTime deadline, string title, string description, string nboard, string idtask)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// A method that gets an email and a board name, returns its board object if its in the facade, else returns null
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public BoardBL GetBoard(string email,string name)
        {
     
            if (!this.boards.ContainsKey(email)) // if user is not in the facade
            {
                Log.Warn("Access to non existent user's board in BOARDFACADE");
                throw new Exception("User isnt registered in board Facade");
            }

            List<BoardBL> userBoards = this.boards[email];
            for (int i = 0; i < userBoards.Count; i++)
            {
                if (userBoards[i].Name.Equals(name))
                {
                    return userBoards[i];
                    
                }
            }

            
           Log.Info("Cant locate Board:" + name + " in facade - BOARDFACADE");
           throw new Exception("Cant locate Board:" + name + " in facade");
        }
        /// <summary>
        /// A method that gets a column details and returns it columnBL object.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="bName"></param>
        /// <param name="col"></param>
        /// <returns>
        /// The input column task limit, if column is leagal.
        /// </returns>
        /// <exception cref="Exception"></exception>
        public ColumnBL GetColByID(string email,string bName, int col)
        {
            BoardBL b = this.GetBoard(email,bName);
            if(col<0 || col>2)
            {
                Log.Warn("Ileagal column Id accessed - BOARDFACADE");
                throw new Exception("column ID is ileagal");
            }

            return b.Columns[col];
        }

        public List<TaskBL> GetInProgressList()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// A method that returns a taskListSL from taskListBL list
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
