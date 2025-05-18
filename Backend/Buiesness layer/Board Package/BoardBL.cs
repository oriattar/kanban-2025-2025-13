
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;




namespace IntroSE.Kanban.Backend.Buiesness_layer
{
    internal class BoardBL
    {
        public static readonly int BACKLOG = 0;
        public static readonly int IN_PROGRESS = 1;
        public static readonly int DONE = 2;
        public static readonly int NO_LIMIT = -1;
        private static readonly int DESCRIPTION_LIMIT = 300;



        private string name;
        private Dictionary<int, ColumnBL> columns;
        private int boardID;
        private string owner;
        private List<string> activeUsers;
  

        /// <summary>
        /// Board BL constructor, gets name of board, and int limit to set the fields
        /// </summary>
        /// <param name="name"></param>
        /// <param name="intpercol"></param>
        public BoardBL(string name , int ID,string owner)
        {
            this.name = name;
            

            this.columns = new Dictionary<int, ColumnBL>();

            columns.Add(BACKLOG, new ColumnBL("backlog"));
            columns.Add(IN_PROGRESS, new ColumnBL("in progress"));
            columns.Add(DONE, new ColumnBL("done"));

            this.boardID = ID;
            this.owner = owner;
            this.activeUsers = new List<string>();
            activeUsers.Add(owner);
        }


        public string Name { get => name; set => name = value; }
        public Dictionary<int, ColumnBL> Columns { get => columns; set => columns = value; }
        public int BoardID { get => boardID; set => boardID = value; }
        public List<string> ActiveUsers { get => activeUsers; }
        public string Owner { get => owner; set => owner = value; }

        /// <summary>
        /// A method that gets collumn id and sets it limit to input limit, if possible.
        /// </summary>
        /// <param name="col"></param>
        /// <param name="limit"></param>
        public void setLimit(int col,int limit)
        {
            
            if (limit < NO_LIMIT || col < BACKLOG || col > DONE) //allows removing the limit aswell
                throw new Exception("Ileagal collumn number or limit");

            ColumnBL c = this.Columns[col];

            if (c.Tasks.Count > limit && limit !=NO_LIMIT)
                throw new Exception("Cant set the new limit, as there are more tasks than the new limit");
           

            
            c.TaskLimit = limit;
        }
       
        /// <summary>
        /// A method that gets a source column and a task thats in the board, and transfer it to the next column
        /// Cant transfer past 2, and checks if the destination column is not full.
        /// </summary>
        /// <param name="sourceCol"></param>
        /// <param name="taskID"></param>
        /// <exception cref="Exception"></exception>
        public void AdvanceTask(int sourceCol ,int taskID)
        {
            if (sourceCol < BACKLOG || sourceCol > IN_PROGRESS)
                throw new Exception("Invalid column ID, or trying to advance past the column limit (past done)");

            TaskBL t = this.columns[sourceCol].FindTask(taskID); // if not found throws an exception

            this.columns[sourceCol].transferTask(this.columns[sourceCol+1],t); // trasfers only if destination is not full
        }

        public void CreateTask(string title, string description, DateTime dueDate, int taskID)
        {
            if(string.IsNullOrEmpty(title))
                throw new Exception("Trying to create a task with empty title");
            if(description == null || description.Length > DESCRIPTION_LIMIT)
                throw new Exception("Trying to create a task with description that is null or past 300 characters");
            if (dueDate < DateTime.Now)
                throw new Exception("Creating a task with invalid due date");

            TaskBL t =new TaskBL(taskID,DateTime.Now,dueDate,title,description);
            this.columns[0].addTask(t);
        }

        /// <summary>
        /// A method that divides to cases and updates the task accordingly
        /// If deadline is minValue it means we are not editing tasksDeadline
        /// if title is null we are not editing title
        /// if desc is null we are not editing description
        /// 
        /// input valitidy happends in TaskBL->Update
        /// </summary>
        /// <param name="deadline"></param>
        /// <param name="title"></param>
        /// <param name="descripition"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public void EditTask(DateTime deadline, string title, string descripition,int col ,int taskId)
        {
            if (col < BACKLOG || col >= DONE)
            {
                throw new Exception("Cant edit a with the given column." + col);
            }

            TaskBL toEdit = this.columns[col].FindTask(taskId);
            if (title != null)
                toEdit.EditTitle(title); // to be implemented
            else if(deadline != DateTime.MinValue)
                toEdit.EditDueDate(deadline);
            else
                toEdit.EditDescription(descripition);
        }

        /// <summary>
        /// Returns the in the task list of the inProgress column
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<TaskBL> GetInProgress()
        {
            return this.columns[IN_PROGRESS].Tasks;
        }


    }
}
