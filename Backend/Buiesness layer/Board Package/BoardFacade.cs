using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using log4net;
using log4net.Util;

namespace IntroSE.Kanban.Backend.Buiesness_layer
{
    public class BoardFacade
    {
        private readonly SystemController systemController;
        private Dictionary<string, List<BoardBL>> boards;
        private AuthenticationFacade aFacade;
        private int taskIdGenerator;
        private int boardIdGenerator;

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        

        /// <summary>
        /// A constructor to the boardFacade, inits the dict
        /// </summary>
        /// <param name="aFacade"></param>
        public BoardFacade(AuthenticationFacade aFacade,SystemController s)
        {
            this.systemController = s;
            this.boards = new Dictionary<string, List<BoardBL>>();
            this.aFacade = aFacade;
            this.taskIdGenerator = SystemController.DEFULT_GENERATOR;
            this.boardIdGenerator = SystemController.DEFULT_GENERATOR;
            LoadAllBoards();
        }

        /// <summary>
        /// Loads all information from the database
        /// </summary>
        public void LoadAllBoards()
        {
            this.boardIdGenerator = this.systemController.LoadBoardIDGen();
            this.taskIdGenerator = this.systemController.LoadTaskIDGen();
            List<BoardDAL> boards = new BoardController().SelectAll(); //reading all the boards
            foreach (BoardDAL b in boards)
            {
                BoardBL toSave = new BoardBL(b); //for each one we construct a new boardbl
                foreach (string user in toSave.ActiveUsers)
                {
                    this.RegisterInFacade(user); //register its users
                    this.boards[user].Add(toSave);//and adding it to its list
                }
            }
        }

        /// <summary>
        /// A method that conductos the board creation operation
        /// *MOVED PREVIOUSLY FROM USERFACADE*
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <exception cref="Exception"></exception>
        public void CreateBoard(string name, string email)
        {
            if (!this.aFacade.IsLoggedIn(email))
            {
                Log.Warn("Cant create a board while not logged in. - BOARDFACADE");
                throw new Exception("Cant create a board while not logged in.");
            }
            if (string.IsNullOrEmpty(name))
            {
                Log.Warn("Cant create a board with no name - BOARDFACADE");
                throw new Exception("Cant create a board with no name");
            }
            if (this.isBoardExist(email, name)) // the boards already exist
            {
                Log.Warn("Board already exist on User's boardfacade entry - BOARDFACADE");
                throw new Exception("Board already exist on User's boardfacade entry");
            }

            BoardBL b = new BoardBL(name,this.boardIdGenerator,email); // creating a new board
            this.boardIdGenerator++; // advances the generator
            this.systemController.UpdateGenerators(this.boardIdGenerator,this.taskIdGenerator);

            Log.Info("A new board:" + name + " to the user:" + email);
            this.AddBoardToUserList(email, b); // adding and ensuring no name duplication
        }

        /// <summary>
        /// Checks if a board exsit on a user's list
        /// </summary>
        /// <param name="user"></param>
        /// <param name="boardName"></param>
        /// <returns></returns>
        private bool isBoardExist(string user,string boardName)
        {
            List<BoardBL> userBoards = this.GetUserBoards(user); //sanity check
            foreach (BoardBL board in userBoards) //checks whether a board is already exist
            {
                if (board.Name.ToLower().Equals(boardName.ToLower())) // the boards already exist
                {
                    return true;
                }
            }
            return false;
        }

        private void AddBoardToUserList(string email, BoardBL toAdd)
        {
            List<BoardBL> userBoards = this.GetUserBoards(email); //sanity check
            if (this.isBoardExist(email,toAdd.Name)) // the boards already exist
            {
                    Log.Warn("Board already exist on User's boardfacade entry - BOARDFACADE");
                    throw new Exception("Board already exist on User's boardfacade entry");
            }
            

            userBoards.Add(toAdd);
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

            BoardBL toDelete = this.GetBoard(email, bName);
            if(!toDelete.Owner.Equals(email))
            {
                Log.Warn("Board deletion was initiated by non owner - BOARDFACADE");
                throw new Exception("Board deletion was initiated by non owner.");
            }

            
            toDelete.Delete();//deleted from db
            this.DeleteForAllUsers(toDelete); //deletes the board from users boardlist
            Log.Info("A board was deleted to the user:" + email + " in BOARDFACADE");
        }

        /// <summary>
        /// A method that itrates over an input board's user list,and removes it for each of them
        /// </summary>
        /// <param name="toDelete"></param>
        private void DeleteForAllUsers(BoardBL toDelete)
        {
            List<string> boardUsers = toDelete.ActiveUsers;
            foreach (string user in boardUsers)
            {
                this.RemoveBoardFromUserList(user, toDelete);
            }
        }

        private void RemoveBoardFromUserList(string email,BoardBL toDelete)
        {
            List<BoardBL> uBoards = this.GetUserBoards(email); // Going throuth sanity check
            uBoards.Remove(toDelete);
        }
        /// <summary>
        /// Registers a user's boardlist if one dosnt exist for the user in the facade
        /// </summary>
        /// <param name="b"></param>
        /// <exception cref="Exception"></exception>
        public void RegisterInFacade(string email)
        {
            if(!this.aFacade.isRegisterd(email))
            {
                Log.Warn("User isnt registerd - cant create an entery");
                throw new Exception("User isnt registerd - cant create an entery");
            }
            if (!this.boards.ContainsKey(email))
            {
                this.boards.Add(email, new List<BoardBL>());
                Log.Info("A list was added to the user:" + email + " in BOARDFACADE");
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
        public void AdvanceTasK(string email, string bName, int col, int taskId)
        {
            if (!this.aFacade.IsLoggedIn(email))
            {
                Log.Warn("Trying to advance a task while not logged in. - BOARDFACADE");
                throw new Exception("Trying to advance a task while not logged in.");
            }
           
            BoardBL targetBoard = this.GetBoard(email, bName);
            TaskBL t = targetBoard.GetTask(col, taskId);

            if (!t.Asignee.Equals(email))
            {
                Log.Warn("Cant advance a task while not beeing its asignee. - BOARDFACADE");
                throw new Exception("Cant advance a task while not beeing its asignee");
            }
           targetBoard.AdvanceTask(col, taskId);
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

            BoardBL targetBoard = this.GetBoard(email, boardName);

            if (!targetBoard.isActiveMember(email))
            {
                Log.Warn("Cant create a task while not an active member - BOARDFACADE");
                throw new Exception("Cant create a task while not an active member.");
            }

            targetBoard.CreateTask(title,description,dueDate,this.taskIdGenerator);
            this.taskIdGenerator++; //advances the id generator
            this.systemController.UpdateGenerators(this.boardIdGenerator, this.taskIdGenerator); // saves in the db
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
        public void EditTask(string email, string boardName, int col, int taskId, DateTime dueDate, string title, string desc) // needs to be tested when tasks are implemented
        {
            if (!this.aFacade.IsLoggedIn(email))
            {
                Log.Warn("Cant edit a task while not logged in. - BOARDFACADE");
                throw new Exception("Cant edit a task while not logged in.");
            }

            BoardBL targetBoard = this.GetBoard(email, boardName);
            TaskBL t = targetBoard.GetTask(col, taskId); // finds the board and the task
            if(!targetBoard.isActiveMember(email))
            {
                Log.Warn("Cant edit a task while not an active user. - BOARDFACADE");
                throw new Exception("Cant edit a task while not an active user.");
            }

            if (col != BoardBL.DONE && !t.Asignee.Equals(email))
            {
                Log.Warn("A task that is not done can only be changed by its asignee. - BOARDFACADE");
                throw new Exception("A task that is not done can only be changed by its asignee");
            }

            targetBoard.EditTask(dueDate, title, desc, col, taskId);
        }

        /// <summary>
        /// A method that gets an email and a board name, returns its board object if its in the facade, else returns null
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public BoardBL GetBoard(string email, string name)
        {
            List<BoardBL> userBoards = this.GetUserBoards(email); // also checks if user exist in facade
            for (int i = 0; i < userBoards.Count; i++)
            {
                if (userBoards[i].Name.Equals(name))
                {
                    return userBoards[i];

                }
            }


            Log.Info("Cant locate Board:" + name + " in the user's facade entry - BOARDFACADE");
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


            List<BoardBL> uBoards = this.GetUserBoards(email);
            List<TaskBL> res = new List<TaskBL>();


            foreach (BoardBL b in uBoards) //going over all of the users boards
            {
                res.AddRange(b.GetInProgress(email)); // building the result list
            }
            return res;

        }

        /// <summary>
        /// A method that gets a user's email, and iterating over its boards.
        /// Adding each board ID to the result list and returns it
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<int> CollectBoardIds(string email)
        {
            if (!this.aFacade.IsLoggedIn(email))
            {
                Log.Warn("Cant collect boards while not logged in. - BOARDFACADE");
                throw new Exception("Cant collect boards while not logged in.");
            }
            List<BoardBL> boards = this.GetUserBoards(email);

            List<int> res = new List<int>();

            foreach (BoardBL b in boards)
            {
                res.Add(b.BoardID);
            }
            return res;
        }

        /// <summary>
        /// A helper method that returns the user list of boardBL objects
        /// mainly used for checking if the user exsit on facade, to avoid duplicating code.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<BoardBL> GetUserBoards(string email)
        {
            if (!this.boards.ContainsKey(email))
            {
                Log.Warn("Cannot locate user in board facade. - BOARDFACADE");
                throw new Exception("Cannot locate user in board facade.");
            }

            return this.boards[email];
        }

        /// <summary>
        /// A method that locate a board in the facade by its ID.
        /// itrating over the entries in the facade and searching the id in each.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private BoardBL GetBoardById(int id)
        {
            foreach(KeyValuePair<string,List<BoardBL>> entry in this.boards)
            {
                List<BoardBL> userBoards = entry.Value;
                
                foreach (BoardBL b in userBoards)
                {
                    if(b.BoardID == id)
                        return b;
                }
            }

            Log.Warn("Cannot locate board by ID. - BOARDFACADE");
            throw new Exception("Cannot locate board by ID");
        }
        /// <summary>
        /// Returns a board's name using its ID, going through a sanity check to see if it exist in facade
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetBoardNameById(int id)
        {
            return this.GetBoardById(id).Name;
        }


        /// <summary>
        /// A method that
        /// </summary>
        /// <param name="toAddEmail"></param>
        /// <param name="boardId"></param>
        /// <exception cref="Exception"></exception>
        public void AddUserAsMember(string toAddEmail,int boardId)
        {
            if (!this.aFacade.IsLoggedIn(toAddEmail))
            {
                Log.Warn("Cant join board while not logged in. - BOARDFACADE");
                throw new Exception("Cant join board while not logged in.");
            }

            BoardBL targetBoard = this.GetBoardById(boardId); // locates the boards


            targetBoard.AddUserAsMember(toAddEmail); // adding the user email to the board
            this.AddBoardToUserList(toAddEmail,targetBoard); // adding the board to the input user list
            Log.Info("User " + toAddEmail + " joined board:" + targetBoard.Name);
        }

        /// <summary>
        /// A method that removes the input user from the board user list, and the board from the user's boardlist.
        /// Also, setting all his assigned tasks to unassinged
        /// </summary>
        /// <param name="emailToRemove"></param>
        /// <param name="boardId"></param>
        /// <exception cref="Exception"></exception>
        public void RemoveUserAsMember(string emailToRemove,int boardId)
        {
            if (!this.aFacade.IsLoggedIn(emailToRemove))
            {
                Log.Warn("Cant leave a board while not logged in. - BOARDFACADE");
                throw new Exception("Cant leave a board while not logged in.");
            }

            BoardBL targetBoard = this.GetBoardById(boardId); // locates the boards

            if(targetBoard.Owner.Equals(emailToRemove))
            {
                Log.Warn("Cant leave a board while beeing the owner. - BOARDFACADE");
                throw new Exception("Cant leave a board while beeing the owner.");
            }

            targetBoard.RemoveUserAsMember(emailToRemove);
            this.RemoveBoardFromUserList(emailToRemove,targetBoard); // removes board from the user list

            targetBoard.UnassingTasks(emailToRemove); // unassigns the leaving user tasks upon leaving
            Log.Info("User " +  emailToRemove + " left board:" + targetBoard.Name);

        }

        /// <summary>
        /// A method that finds the target board and transfer
        /// </summary>
        /// <param name="currentOwnerEmail"></param>
        /// <param name="newOwnerEmail"></param>
        /// <param name="boardName"></param>
        /// <exception cref="Exception"></exception>
        public void TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            if (!this.aFacade.IsLoggedIn(currentOwnerEmail)) // only transfering user needs to be logged in
            {
                Log.Warn("Cant transfer owmership of a board while not logged in. - BOARDFACADE");
                throw new Exception("Cant transfer owmership of a board while not logged in.");
            }
            BoardBL targetBoard = this.GetBoard(currentOwnerEmail, boardName); // locate target board
            if (currentOwnerEmail.Equals(newOwnerEmail) || targetBoard.Owner.Equals(newOwnerEmail))
            {
                Log.Warn("Cant transfer owmership -Already the owner. - BOARDFACADE");
                throw new Exception("Cant transfer owmership -Already the owner.");

            }


            targetBoard.TransferOwnership(newOwnerEmail); // changes the owner
            Log.Info("Ownership was transferd from " + currentOwnerEmail + " to " +  newOwnerEmail+" - on board:" + boardName);
        }

        /// <summary>
        /// A method that operates the assignemt of a task by a board memeber to another board memeber
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="col"></param>
        /// <param name="taskID"></param>
        /// <param name="emailAssignee"></param>
        /// <exception cref="Exception"></exception>
        public void AssignTaskToMember(string email, string boardName, int col, int taskID, string emailAssignee)
        {
            if (!this.aFacade.IsLoggedIn(email))
            {
                Log.Warn("Cant assign a task while not logged in. - BOARDFACADE");
                throw new Exception("Cant assign a task while not logged in.");
            }

            BoardBL targetBoard = this.GetBoard(email,boardName);
            if(!targetBoard.isActiveMember(email) || !targetBoard.isActiveMember(emailAssignee)) // bouth must be active users of the board
            {
                Log.Warn("Either the assigner or the asignee is not an active member in the board. - BOARDFACADE");
                throw new Exception("Either the assigner or the asignee is not an active member in the board");
            }
            TaskBL t = targetBoard.GetTask(col,taskID);
            if(t.Asignee != "unassigned" && !email.Equals(t.Asignee)) //once a task is assinged, only its assignee can assign to someone else
            {
                Log.Warn("While the task is assinged to a member, only its assignee can change its assignment. - BOARDFACADE");
                throw new Exception("While the task is assinged to a member, only its assignee can change its assignment.");
            }

            t.Asignee = emailAssignee; //assigns the task.
        }

        /// <summary>
        /// A method returning a list of boardBL
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<BoardBL> GetBoards(string email)
        {
            if (!this.aFacade.IsLoggedIn(email))
            {
                Log.Warn("Cant collect boards while not logged in. - BOARDFACADE");
                throw new Exception("Cant collect boards while not logged in.");
            }

            this.RegisterInFacade(email); // first of checks if a user needs to be registered.
            List<int> ids = this.CollectBoardIds(email);
            List<BoardBL> res = new List<BoardBL>();

            foreach (int id in ids)
            {
                res.Add(this.GetBoardById(id));
            }

            return res;
        }

        /// <summary>
        /// Collects active users from the input board and returns it.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<string> GetActiveUsers(string email,string boardName)
        {
            if (!this.aFacade.IsLoggedIn(email))
            {
                Log.Warn("Cant collect active users while not logged in. - BOARDFACADE");
                throw new Exception("Cant collect active users while not logged in.");
            }

            return this.GetBoard(email,boardName).ActiveUsers;
        }

        /// <summary>
        /// Method that allowes adding a member to a board while he is offline.
        /// </summary>
        /// <param name="toAddEmail"></param>
        /// <param name="boardId"></param>
        public void OfflineJoinBoard(string toAddEmail, int boardId)
        {
            BoardBL targetBoard = this.GetBoardById(boardId); // locates the boards

            targetBoard.AddUserAsMember(toAddEmail); // adding the user email to the board
            this.AddBoardToUserList(toAddEmail, targetBoard); // adding the board to the input user list
            Log.Info("User " + toAddEmail + " joined board:" + targetBoard.Name);
        }
    }
}
