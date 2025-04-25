using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.Buiesness_layer
{
    public class BoardBL
    {
        private string name;
        private int intpercol;
        private List<TaskBL> backlog;
        private List<TaskBL> ongoing;
        private List<TaskBL> done;
        private long nextTaskId;

        /// <summary>
        /// Board BL constructor, gets name of board, and int limit to set the fields
        /// </summary>
        /// <param name="name"></param>
        /// <param name="intpercol"></param>
        public BoardBL(string name, int intpercol)
        {
            this.name = name;
            this.intpercol = intpercol;
            this.backlog = new List<TaskBL>();
            this.ongoing = new List<TaskBL>();
            this.done = new List<TaskBL>();
            this.nextTaskId = 0;
        }

        public string Name { get => name; set => name = value; }
        public int Intpercol { get => intpercol; set => intpercol = value; }
        internal List<TaskBL> Backlog { get => backlog; set => backlog = value; }
        internal List<TaskBL> Ongoing { get => ongoing; set => ongoing = value; }
        internal List<TaskBL> Done { get => done; set => done = value; }
        public long NextTaskId { get => nextTaskId; set => nextTaskId = value; }


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
