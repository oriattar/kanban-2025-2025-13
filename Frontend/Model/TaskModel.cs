using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Frontend.Model
{
    public class TaskModel
    {
        private string title;
        private string assignedTo;
        private int taskID;
        private DateTime creationTime;
        private DateTime dueDate;
        private int colID;

        /// <summary>
        /// Constructs a task model from task sl object
        /// </summary>
        /// <param name="t"></param>
        public TaskModel(TaskSL t)
        {
            this.title = t.Title;
            this.assignedTo = t.Asignee;
            this.creationTime = t.CreationTime;
            this.dueDate = t.DueDate;
            this.taskID = t.Id;
            this.colID = t.ColID;
        }

        public string Title { get => title; set => title = value; }
        public string AssignedTo { get => assignedTo; set => assignedTo = value; }
        public DateTime CreationTime { get => creationTime; set => creationTime = value; }
        public DateTime DueDate { get => dueDate; set => dueDate = value; }
        public int TaskID { get => taskID;}
        public int ColID { get => colID; set => colID = value; }
    }
}
