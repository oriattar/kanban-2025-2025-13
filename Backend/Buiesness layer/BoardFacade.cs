using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using IntroSE.Kanban.Backend.ServiceLayer;
using log4net;

namespace IntroSE.Kanban.Backend.Buiesness_layer
{
    internal class BoardFacade
    {
        private Dictionary<string, List<BoardBL>> boards;
        private AuthenticationFacade aFacade;
        private int taskIdGenerator;

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        

        /// <summary>
        /// A constructor to the boardFacade, inits the dict
        /// </summary>
        /// <param name="aFacade"></param>
        public BoardFacade(AuthenticationFacade aFacade)
        {
            this.boards = new Dictionary<string, List<BoardBL>>();
            this.aFacade = aFacade;
            this.taskIdGenerator = 0;
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
            if (!this.aFacade.IsLoggedIn(email))
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
                this.boards.Add(user.Email, user.Boards);
                Log.Info("A list was added to the user:" + user.Email + " in BOARDFACADE");
            }
        }

        /// <summary>
        /// A method that finds the board and operates its advance task method.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="bName"></param>
        /// <param name="col"></param>
        /// <param name="taskId"></param>
        /// <exception cref="Exception"></exception>
        public void advanceTasK(string email, string bName, int col, int taskId)
        {
            if (!this.aFacade.IsLoggedIn(email))
            {
                Log.Warn("Trying to advance a task while not logged in. - BOARDFACADE");
                throw new Exception("Trying to advance a task while not logged in.");
            }

            this.GetBoard(email, bName).AdvanceTask(col, taskId);
        }

        /// <summary>
        /// A method that finds the board and creating a new task in it, if all the parameters are valid.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="dueDate"></param>
        /// <exception cref="Exception"></exception>
        public void CreateTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            if(!this.aFacade.IsLoggedIn(email))
            {
                Log.Warn("Trying to add a task while not logged in. - BOARDFACADE");
                throw new Exception("Trying to add a task while not logged in.");
            }

            this.GetBoard(email, boardName).CreateTask(title,description,dueDate,this.taskIdGenerator);
            this.taskIdGenerator++; //advances the id generator
            Log.Info("A task was added to the board" + boardName + " in BOARDFACADE");
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
            if (!this.aFacade.IsLoggedIn(email))
            {
                Log.Warn("Cant set limit while not logged in. - BOARDFACADE");
                throw new Exception("Cant set limit while not logged in.");
            }

            BoardBL board = this.GetBoard(email,bName);

            board.setLimit(col,limit);
            Log.Info("A Limit was set to the col:" + col+ " for the board: " + bName + " in BOARDFACADE");

        }

        /// <summary>
        /// A method that operates the task edition operation
        
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="col"></param>
        /// <param name="taskId"></param>
        /// <param name="dueDate"></param>
        /// <param name="title"></param>
        /// <param name="desc"></param>
        /// <exception cref="Exception"></exception>
        public void EditTask(string email, string boardName, int col, int taskId, DateTime dueDate, string title,string desc) // needs to be tested when tasks are implemented
        {
            if (!this.aFacade.IsLoggedIn(email))
            {
                Log.Warn("Cant edit a task while not logged in. - BOARDFACADE");
                throw new Exception("Cant edit a task while not logged in.");
            }
            if(col<0 || col >=2)
            {
                Log.Warn("Cant edit a with the given column." + col + " - BOARDFACADE");
                throw new Exception("Cant edit a with the given column." + col);
            }

            this.GetBoard(email,boardName).EditTask(dueDate,title,desc,col,taskId); // finds the board and calls the edit task operation
        }

        /// <summary>
        /// A method that gets an email and a board name, returns its board object if its in the facade, else returns null
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public BoardBL GetBoard(string email, string name)
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
        public ColumnBL GetColByID(string email, string bName, int col)
        {
            if (!this.aFacade.IsLoggedIn(email))
            {
                Log.Warn("Cant get the column while not logged in. - BOARDFACADE");
                throw new Exception("Cant get the column while not logged in.");
            }
            BoardBL b = this.GetBoard(email,bName);
            if(col<0 || col>2)
            {
                Log.Warn("Ileagal column Id accessed - BOARDFACADE");
                throw new Exception("column ID is ileagal");
            }

            return b.Columns[col];
        }

        /// <summary>
        /// A method that builds the inProgress list of TaskBL objects.
        /// going over all of the boards of the input users, and adding the in progress column of each board to the result.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<TaskBL> GetInProgressList(string email)
        {
            if (!this.aFacade.IsLoggedIn(email))
            {
                Log.Warn("Cant get the inProgress list while not logged in. - BOARDFACADE");
                throw new Exception("Cant get the inProgress list while not logged in.");
            }
            if (!this.boards.ContainsKey(email))
            {
                Log.Warn("Cant get inProgress list, user isnt registerd in facade. - BOARDFACADE");
                throw new Exception("Cant get inProgress list, user isnt registerd in facade");
            }


            List<TaskBL> res = new List<TaskBL>();
            List<BoardBL> uBoards = this.boards[email];


            foreach (BoardBL b in uBoards) //going over all of the users boards
            {
                res.AddRange(b.GetInProgress()); // building the result list
            }
            return res;
        }
    }
}
