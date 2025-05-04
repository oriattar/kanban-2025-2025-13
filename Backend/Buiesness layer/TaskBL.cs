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
        private int id;
        private DateTime creationtime;
        private DateTime deadline;
        private string title;
        private string description;

        public TaskBL(int id, DateTime creationtime, DateTime deadline, string title, string description)
        {
            this.id = id;
            this.creationtime = creationtime;
            this.deadline = deadline;
            this.title = title;
            this.description = description;
        }

        public int Id { get => id; set => id = value; }
        public DateTime Creationtime { get => creationtime; set => creationtime = value; }
        public DateTime Deadline { get => deadline; set => deadline = value; }
        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }

        /*
         * 
         * 
         * 
         * 
         * When implimenting those methods, input must be check according to the requierments.
         * 
         * 
         * 
         * 
         */
        public void UpdateTitle(string title)
        {
            throw new NotImplementedException();
        }

        public void UpdateDescription(string desc)
        {
            throw new NotImplementedException();
        }

        public void UpdateDeadline(DateTime d)
        {
            throw new NotImplementedException();
        }


    }
}