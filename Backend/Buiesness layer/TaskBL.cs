using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.Backend.Buiesness_layer
{
    public class TaskBL
    {
        private long id;
        private DateTime creationtime;
        private DateTime deadline;
        private string title;
        private string description;

        public TaskBL(long id, DateTime creationtime, DateTime deadline, string title, string description)
        {
            this.id = id;
            this.creationtime = creationtime;
            this.deadline = deadline;
            this.title = title;
            this.description = description;
        }

        public long Id { get => id; set => id = value; }
        public DateTime Creationtime { get => creationtime; set => creationtime = value; }
        public DateTime Deadline { get => deadline; set => deadline = value; }
        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }

        public void EditTitle(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
                throw new ArgumentException("Title cannot be empty.");
            if (newTitle.Length > 50)
                throw new ArgumentException("Title too long.");
            title = newTitle;
        }

        public void EditDescription(string newDescription)
        {
            if (newDescription.Length > 300)
                throw new ArgumentException("Description too long.");
            description = newDescription;
        }

        public void EditDueDate(DateTime newDeadline)
        {
            if (newDeadline < creationtime)
                throw new ArgumentException("Deadline cannot be before creation time.");
            deadline = newDeadline;
        }

        public void Advance()
        {
            
        }
    }
}
