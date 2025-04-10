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
        private List<TaskBL> backing;
        private List<TaskBL> ongoing;
        private List<TaskBL> done;
        private long nextTaskId;

        public BoardBL(string name, int intpercol, List<TaskBL> backing, List<TaskBL> ongoing, List<TaskBL> done, long nextTaskId)
        {
            this.name = name;
            this.intpercol = intpercol;
            this.backing = backing;
            this.ongoing = ongoing;
            this.done = done;
            this.nextTaskId = nextTaskId;
        }

        public string Name { get => name; set => name = value; }
        public int Intpercol { get => intpercol; set => intpercol = value; }
        internal List<TaskBL> Backing { get => backing; set => backing = value; }
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
