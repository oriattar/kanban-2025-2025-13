using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buiesness_layer;



namespace IntroSE.Kanban.Backend.ServiceLayer
{

    public class TaskSL
    {
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime DueDate { get; set; }
        public string Title{ get; set; }
        public string Description { get; set; }

        public string Asignee { get;set; }


        

        public TaskSL(int id, DateTime creationtime, DateTime deadline, string title, string description)
        {
            Id = id;
            this.CreationTime = creationtime;
            this.DueDate = deadline;
            this.Title = title;
            this.Description = description;
        }
        
        public TaskSL(TaskBL t)
        {
            this.Id = t.Id;
            this.CreationTime = t.Creationtime;
            this.DueDate = t.Deadline;
            this.Title = t.Title;
            this.Description = t.Description;
            this.Asignee = t.Asignee;
        }

        public TaskSL() { }
    }
}
