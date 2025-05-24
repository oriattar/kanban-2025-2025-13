using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.Backend.Buiesness_layer
{
    
    internal class TaskBL
    {
        private static readonly int DESCRIPTION_LIMIT = 300;
        private static readonly int TITLE_LIMIT = 50;

        private int id;
        private DateTime creationtime;
        private DateTime deadline;
        private string title;
        private string description;
        private string asignee;

        public TaskBL(int id, DateTime creationtime, DateTime deadline, string title, string description)
        {
            this.id = id;
            this.creationtime = creationtime;
            this.deadline = deadline;
            this.title = title;
            this.description = description;
            this.asignee = "unassigned";
        }

        public int Id { get => id; set => id = value; }
        public DateTime Creationtime { get => creationtime; set => creationtime = value; }
        public DateTime Deadline { get => deadline; set => deadline = value; }
        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }
        public string Asignee { get => asignee; set => asignee = value; }

        public void EditTitle(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
                throw new ArgumentException("Title cannot be empty.");
            if (newTitle.Length > TITLE_LIMIT)
                throw new ArgumentException("Title too long.");
            title = newTitle;
        }

        public void EditDescription(string newDescription)
        {
            if (newDescription.Length > DESCRIPTION_LIMIT)
                throw new ArgumentException("Description too long.");
            description = newDescription;
        }

        public void EditDueDate(DateTime newDeadline)
        {
            if (newDeadline <= creationtime)
                throw new ArgumentException("Deadline cannot be before creation time.");
            deadline = newDeadline;
        }
        /// <summary>
        /// See full implemintation instructions
        /// </summary>
        public void setAsUnassigned()
        {
            this.Asignee = "unassigned";
        }
    }
}
