using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class TaskSL
    {
        private long id;
        private DateTime creationtime;
        private DateTime deadline;
        private string title;
        private string description;

        public long Id { get => id; set => id = value; }
        public DateTime Creationtime { get => creationtime; set => creationtime = value; }
        public DateTime Deadline { get => deadline; set => deadline = value; }
        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }

        public TaskSL(long id, DateTime creationtime, DateTime deadline, string title, string description)
        {
            Id = id;
            this.Creationtime = creationtime;
            this.Deadline = deadline;
            this.Title = title;
            this.Description = description;
        }
    }
}
