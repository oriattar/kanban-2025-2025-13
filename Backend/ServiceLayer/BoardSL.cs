using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buiesness_layer;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class BoardSL
    {
        public string name;
        public int intpercol;
        public List<TaskSL> backlog;
        public List<TaskSL> ongoing;
        public List<TaskSL> done;


        /// <summary>
        /// A consturctor for the boardSL
        /// </summary>
        /// <param name="b"></param>
        public BoardSL(BoardBL b)
        {
            this.name = b.Name;
            this.intpercol = b.Intpercol;
            this.backlog = Convert(b.Backlog);
            this.ongoing = Convert(b.Ongoing);
            this.done = Convert(b.Done);
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public int Limit
        {
            get { return this.intpercol; }
            set { this.intpercol = value; }
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
        /// <summary>
        /// Helper method that converts List of taskBL to list of TaskSL
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static List<TaskSL> Convert (List<TaskBL> list)
        {
            List<TaskSL > result = new List<TaskSL>();
            foreach (TaskBL b in list)
            {
                result.Add(new TaskSL(b));
            }

            return result;
        }

    }
}
