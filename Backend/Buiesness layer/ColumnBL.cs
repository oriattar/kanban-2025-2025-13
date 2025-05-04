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
            this.taskLimit = -1;
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

        /// <summary>
        /// A method that searches for a task within the column with the input id, and returns it.
        /// If the column dosnt conatin a task with the inpurt ID throws and exception/
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public TaskBL FindTask(int id)
        {
            for(int i = 0; i < this.tasks.Count; i++)
            {
                if (this.tasks[i].Id == id)
                    return this.tasks[i];
            }

            throw new Exception("A task with the given ID was not found in the column: " + this.name);
        }

        /// <summary>
        /// A method that transfers a task from 1 column to another.
        /// if the destination is full, an exception is thrown.
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="toTransfer"></param>
        public void transferTask(ColumnBL destination, TaskBL toTransfer)
        {
            destination.addTask(toTransfer); // removes from one column and add to the other, if dest is full throws an exception and not transffering

            this.Tasks.Remove(toTransfer);
        }
    }
}
