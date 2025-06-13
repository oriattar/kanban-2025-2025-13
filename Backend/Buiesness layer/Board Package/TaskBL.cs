using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.Backend.Buiesness_layer
{
    
    public class TaskBL
    {
        private static readonly int DESCRIPTION_LIMIT = 300;
        private static readonly int TITLE_LIMIT = 50;

        private int id;
        private DateTime creationtime;
        private DateTime deadline;
        private string title;
        private string description;
        private string asignee;
        private string colID;

        private TaskDAL taskDTO;

        /// <summary>
        /// A normal constructor, given the needed parameters, at creation of a task
        /// </summary>
        /// <param name="id"></param>
        /// <param name="col"></param>
        /// <param name="creationtime"></param>
        /// <param name="deadline"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        public TaskBL(int id,string col, DateTime creationtime, DateTime deadline, string title, string description)
        {
            this.taskDTO = new TaskDAL(id,col, creationtime, deadline, title, description, "unassigned");
            this.id = id;
            this.creationtime = creationtime;
            this.deadline = deadline;
            this.title = title;
            this.description = description;
            this.asignee = "unassigned";

            taskDTO.Save();
        }
        /// <summary>
        /// Constructs a tasBL from TaskDAL
        /// </summary>
        /// <param name="t"></param>
        public TaskBL(TaskDAL t)
        {
            this.taskDTO = t;
            this.id = t.TaskId;
            this.creationtime = t.CreatedAt;
            this.deadline = t.DueAt;
            this.title = t.TaskTitle;
            this.description = t.TaskDescription;
            this.asignee = t.AssignedUser;
            this.colID = t.ColID;
        }

        public int Id { get => id;}
        public DateTime Creationtime { get => creationtime;}

        /*
         * 
         * Setters, which also responsible to set the new values at the TaskDTO object
         * 
         * 
         */
        public DateTime Deadline
        {
            get => deadline;
            set
            {
                this.deadline = value;
                this.TaskDTO.DueAt = value;
            }
        }
        public string Title
        {
            get => title; set
            {
                this.title = value;
                this.taskDTO.TaskTitle = value;
            }
        }
        public string Description { get => description; 
            set
            {
                this.description = value;
                this.taskDTO.TaskDescription = value;
            }

        }
        public string Asignee { get => asignee; 
            set
            {
                this.asignee = value;
                this.taskDTO.AssignedUser = value;
            }
        }
        public TaskDAL TaskDTO { get => taskDTO; }
        public string ColID
        {
            get => colID; 
            set
            {
                colID = value;
                this.taskDTO.ColID = value;
            }

        }

        /// <summary>
        /// Editing the title, using the property.
        /// </summary>
        /// <param name="newTitle"></param>
        /// <exception cref="ArgumentException"></exception>
        public void EditTitle(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
                throw new ArgumentException("Title cannot be empty.");
            if (newTitle.Length > TITLE_LIMIT)
                throw new ArgumentException("Title too long.");
            Title = newTitle;
        }

        /// <summary>
        /// Editing the description, using the property.
        /// </summary>
        /// <param name="newDescription"></param>
        /// <exception cref="ArgumentException"></exception>
        public void EditDescription(string newDescription)
        {
            if (newDescription != null && newDescription.Length > DESCRIPTION_LIMIT)
                throw new ArgumentException("Description too long.");
            Description = newDescription;
        }

        /// <summary>
        /// Editing the duedate, using the property.
        /// </summary>
        /// <param name="newDeadline"></param>
        /// <exception cref="ArgumentException"></exception>
        public void EditDueDate(DateTime newDeadline)
        {
            if (newDeadline <= creationtime)
                throw new ArgumentException("Deadline cannot be before creation time.");
            Deadline = newDeadline;
        }
        /// <summary>
        /// Sets a task to unassigned,using the property
        /// </summary>
        public void setAsUnassigned()
        {
            this.Asignee = "unassigned";
        }

        /// <summary>
        /// helper method that converts a list form taskdals to taskbls
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<TaskBL> Convert(List<TaskDAL> list)
        {
            List<TaskBL> res = new List<TaskBL>();
            foreach(TaskDAL task in list)
            {
                res.Add(new TaskBL(task));
            }
            return res;
        }
    }
}
