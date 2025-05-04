using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.Buiesness_layer
{
    internal class BoardBL
    {
        private string name;
        private Dictionary<int, ColumnBL> columns;
        private long nextTaskId;

        /// <summary>
        /// Board BL constructor, gets name of board, and int limit to set the fields
        /// </summary>
        /// <param name="name"></param>
        /// <param name="intpercol"></param>
        public BoardBL(string name)
        {
            this.name = name;
            
            this.nextTaskId = 1;

            this.columns = new Dictionary<int, ColumnBL>();

            columns.Add(0, new ColumnBL("backlog"));
            columns.Add(1, new ColumnBL("in progress"));
            columns.Add(2, new ColumnBL("done"));
        }


        public string Name { get => name; set => name = value; }
        public long NextTaskId { get => nextTaskId; set => nextTaskId = value; }
        public Dictionary<int, ColumnBL> Columns { get => columns; set => columns = value; }

        /// <summary>
        /// A method that gets collumn id and sets it limit to input limit, if possible.
        /// </summary>
        /// <param name="col"></param>
        /// <param name="limit"></param>
        public void setLimit(int col,int limit)
        {
            ColumnBL c = this.Columns[col];
            if(c.Tasks.Count > limit && limit !=-1)
            {
                throw new Exception("Cant set the new limit, as there are more tasks than the new limit");
            }

            c.TaskLimit = limit;
        }
        public TaskBL AdvanceTask(string taskID)
        {
            throw new NotImplementedException();
        }

        public TaskBL CreateTask(DateTime deadline, string title, string descripition)
        {
            throw new NotImplementedException();
        }

        public TaskBL EditTask(DateTime deadline, string title, string descripition)
        {
            throw new NotImplementedException();
        }

        public List<TaskBL> GetInProgress()
        {
            throw new NotImplementedException();
        }

    }
}
