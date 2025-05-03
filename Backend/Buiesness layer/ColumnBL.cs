using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.Backend.Buiesness_layer
{
    internal class ColumnBL
    {
        private List<TaskBL> tasks;
        private string name;
        private int taskLimit;

        /// <summary>
        /// A constructor to a collumn object
        /// </summary>
        /// <param name="name"></param>
        public ColumnBL(string name)
        {
            this.name = name;
            tasks = new List<TaskBL>();
        }
        public List<TaskBL> Tasks { get => tasks; set => tasks = value; }
        public string Name { get => name; set => name = value; }
        public int TaskLimit { get => taskLimit; set => taskLimit = value; }

        /// <summary>
        /// A method that allows adding task to a collumn
        /// </summary>
        /// <param name="task"></param>
        public void addTask(TaskBL task)
        {
            if(this.taskLimit != -1 && this.taskLimit == this.tasks.Count) // -1 marks no limit
            {
                throw new Exception("Cannot add task, limit has reached for this collumn:" + this.name);
            }    
            tasks.Add(task);
        }
    }
}
