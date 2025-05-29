using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.Backend.Buiesness_layer
{
    internal class ColumnBL
    {
        public static readonly int NO_LIMIT = -1;


        private List<TaskBL> tasks;
        private string name;
        private int taskLimit;
        private string colID;
        private int boardId;

        private ColumnDAL columnDTO;

        /// <summary>
        /// A constructor to a collumn object
        /// </summary>
        /// <param name="name"></param>
        public ColumnBL(string name,string colID, int boardId)
        {
            this.name = name;
            tasks = new List<TaskBL>();
            this.taskLimit = NO_LIMIT;
            this.colID = colID;
            this.boardId = boardId;

            this.columnDTO = new ColumnDAL(colID,boardId,name,NO_LIMIT);
            this.columnDTO.Save();
        }
        /// <summary>
        /// Constructs a col from a dal object
        /// </summary>
        /// <param name="c"></param>
        public ColumnBL(ColumnDAL c)
        {
            this.columnDTO = c;
            this.name = c.Name;
            this.tasks = TaskBL.Convert(c.Tasks);
            this.taskLimit = c.TaskLimit;
            this.colID=c.ColID;
            this.boardId = c.BoardID;
        }

        public List<TaskBL> Tasks { get => tasks; set => tasks = value; }
        public string Name { get => name; }

        /// <summary>
        /// setter for the limit field, updates to dto aswell.
        /// </summary>
        public int TaskLimit { get => taskLimit; set
            {
                taskLimit = value;
                this.columnDTO.TaskLimit = value;
            }
        }
        public ColumnDAL ColumnDTO { get => columnDTO;  }
        public string ColID { get => colID; set => colID = value; }

        /// <summary>
        /// A method that allows adding task to a collumn
        /// </summary>
        /// <param name="task"></param>
        public void addTask(TaskBL task)
        {
            if(this.taskLimit != NO_LIMIT && this.taskLimit == this.tasks.Count) // -1 marks no limit
            {
                throw new Exception("Cannot add task, limit has reached for this collumn:" + this.name);
            }    
            tasks.Add(task);
            task.ColID = this.colID; //updates the task column
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
            this.ColumnDTO.RemoveTask(toTransfer.TaskDTO); // removes from the dto, its col was already updated in the add task method
        }

        /// <summary>
        /// Setting all the tasks that are assigned to the input user to unassigned.
        /// </summary>
        /// <param name="asigneeToRemove"></param>
        public void UnassignTasks(string asigneeToRemove)
        {
            foreach (TaskBL task in this.Tasks)
            {
                if (task.Asignee.Equals(asigneeToRemove))
                {
                    task.setAsUnassigned();
                }

            }
        }
    }
}
