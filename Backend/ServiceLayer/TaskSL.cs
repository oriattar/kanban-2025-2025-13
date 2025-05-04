using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buiesness_layer;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class TaskSL
    {
        private int id;
        private DateTime creationtime;
        private DateTime deadline;
        private string title;
        private string description;

        public int Id { get => id; set => id = value; }
        public DateTime Creationtime { get => creationtime; set => creationtime = value; }
        public DateTime Deadline { get => deadline; set => deadline = value; }
        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }

        public TaskSL(int id, DateTime creationtime, DateTime deadline, string title, string description)
        {
            Id = id;
            this.Creationtime = creationtime;
            this.Deadline = deadline;
            this.Title = title;
            this.Description = description;
        }
        
        public TaskSL(TaskBL t)
        {
            this.id = t.Id;
            this.creationtime = t.Creationtime;
            this.deadline = t.Deadline;
            this.title = t.Title;
            this.description = t.Description;
        }
    }
}
