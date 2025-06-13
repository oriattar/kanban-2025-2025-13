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
        public readonly int COL_ID_SPLITTER = 1;
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime DueDate { get; set; }
        public string Title{ get; set; }
        public string Description { get; set; }

        public string Asignee { get;set; }
        public int ColID { get => colID; set => colID = value; }

        private int colID;




        public TaskSL(TaskBL t)
        {
            this.Id = t.Id;
            this.CreationTime = t.Creationtime;
            this.DueDate = t.Deadline;
            this.Title = t.Title;
            this.Description = t.Description;
            this.Asignee = t.Asignee;

            string col_id = t.ColID.Split('_')[COL_ID_SPLITTER];
            this.colID = int.Parse(col_id);
        }

        public TaskSL() { }
    }
}
