using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class BoardSL
    {
        public string name;
        public int limit;
        public List<TaskSL> backlog;
        public List<TaskSL> ongoing;
        public List<TaskSL> done;

        public BoardSL()
        {
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public int Limit
        {
            get { return this.limit; }
            set { this.limit = value; }
        }

        public List<TaskSL> Backlog
        {
            get { return this.backlog; }
            set { this.backlog = value; }
        }

        public List<TaskSL> Ongoing
        {
            get { return this.ongoing; }
            set { this.ongoing = value; }
        }

        public List<TaskSL> Done
        {
            get { return this.done; }
            set { this.done = value; }
        }

    }
}
