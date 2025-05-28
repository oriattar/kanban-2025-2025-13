using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buiesness_layer;
using log4net;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class ColumnDAL
    {
        private readonly ColumnController controller;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public const string idColumnName = "idColumn";
        public const string BoardColumnName = "idBoard";
        public const string NameColumnName = "name";
        public const string LimitColumnName = "limit";

        private bool isPresisted;
        private List<TaskDAL> tasks;
        private string name;
        private int taskLimit;
        private string colID;
        private int boardID;

        /// <summary>
        /// Constructor for columnDAL
        /// </summary>
        /// <param name="name"></param>
        /// <param name="limit"></param>
        /// <param name="colID"></param>
        /// <param name="tasks"></param>
        /// <param name="boardID"></param>
        public ColumnDAL(string name, int limit, string colID, List<TaskDAL> tasks, int boardID)
        {
            this.controller = new ColumnController();
            this.name = name;
            this.colID = colID;
            this.taskLimit = limit;
            this.tasks = tasks;
            this.boardID = boardID;
        }
        /// <summary>
        /// A Constructor, for controller usage mainly.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="limit"></param>
        /// <param name="colID"></param>
        /// <param name="boardID"></param>
        public ColumnDAL(string colID, int boardID, string name, int limit)
        {
            this.controller = new ColumnController();
            this.name = name;
            this.colID = colID;
            this.taskLimit = limit;
            this.tasks = new List<TaskDAL>();
            this.boardID = boardID;
        }
        public List<TaskDAL> Tasks { get => tasks; set => tasks = value; }
        public string Name { get => name; }

        /// <summary>
        /// Setter for the taskLimit - saves in the db
        /// </summary>
        public int TaskLimit
        {
            get => taskLimit;
            set
            {
                if (IsPresisted)
                    this.controller.UpdateLimit(this, value);

                this.taskLimit = value;
            }
        }
            
            
        public string ColID { get => colID; }
        public bool IsPresisted { get => isPresisted; set => isPresisted = value; }
        public int BoardID { get => boardID; }

        

        /// <summary>
        /// Saves a dto - inserts it to the db
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void Save()
        {
            if(this.IsPresisted)
            {
                Log.Error("Cannot insert an already presisted column - COLUMNDAL");
                throw new Exception("Cannot insert an already presisted column");
            }
            this.controller.Insert(this);
            IsPresisted = true;
        }
        /// <summary>
        /// Adds a task to a column,and updates its col id.
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(TaskDAL task)
        {
            this.Tasks.Add(task);
            task.ColID = colID;
           
        }

        /// <summary>
        /// Removes task form the column
        /// </summary>
        /// <param name="task"></param>
        public void RemoveTask(TaskDAL task)
        {
            this.Tasks.Remove(task);
        }
    }
}
