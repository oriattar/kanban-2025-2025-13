using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buiesness_layer;
using Microsoft.VisualBasic;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class TaskDAL
    {
        
        /*
         * NAMES OF THE COLUMNS IN THE TABLE
         */

        public const string IdTask = "idTask";
        public const string IdColumn = "idColumn";
        public const string CreationTime = "creationTime";
        public const string DueTime = "dueTime";
        public const string Title = "title";
        public const string Description = "description";
        public const string Asignee = "asignee";


        private TaskController controller;
        private bool isPersisted;


        private int taskId;
        private string colID;
        private DateTime createdAt;
        private DateTime dueAt;
        private string taskTitle;
        private string taskDescription;
        private string assignedUser;

        /// <summary>
        /// Constructor for a taskDAL, given the req parameters, at creating of taskBL
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="col"></param>
        /// <param name="createdAt"></param>
        /// <param name="dueAt"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="asignee"></param>
        public TaskDAL(int taskId,string col, DateTime createdAt, DateTime dueAt, string title, string description, string asignee)
        {
            this.controller = new TaskController();

            this.taskId = taskId;
            this.ColID = col;
            this.createdAt = createdAt;
            this.DueAt = dueAt;
            this.TaskTitle = title;
            this.TaskDescription = description;
            this.AssignedUser = asignee;

            isPersisted = false;
        }

        
        public int TaskId {get => taskId; }
        public DateTime CreatedAt{ get => createdAt; }

        /*
         * 
         * Next method are setters ,which also responsible for updating the files in the db.
         * ---------------------------------------------------------------------------------
         * 
         */
        public string ColID { get => colID;
            set
            {
                if (isPersisted)
                    controller.Update(this.TaskId, IdColumn, value);
                colID = value;
            }
        }


        public DateTime DueAt
        {
            get => dueAt;
            set
            {
                if (isPersisted)
                    controller.Update(this.TaskId,DueTime, value.ToString("yyyy-MM-dd HH:mm:ss"));
                dueAt = value;
            }
        }

        public string TaskTitle
        {
            get => taskTitle;
            set
            {
                if (isPersisted)
                    controller.Update(this.TaskId,Title, value);
                taskTitle = value;
            }
        }

        public string TaskDescription
        {
            get => taskDescription;
            set
            {
                if (isPersisted)
                    controller.Update(this.TaskId, Description, value);
                taskDescription = value;
            }
        }

        public string AssignedUser
        {
            get => assignedUser;
            set
            {
                if (isPersisted)
                    controller.Update(this.TaskId, Asignee, value);
                assignedUser = value;
            }
        }


        /// <summary>
        /// Saves a task in the db, if not presisted
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void Save()
        {
            if (isPersisted)
                throw new InvalidOperationException("Cannot save a persisted task.");

            controller.Insert(this);
            isPersisted = true;
        }


        
    }
}