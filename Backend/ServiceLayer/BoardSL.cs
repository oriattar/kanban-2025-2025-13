using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buiesness_layer;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    internal class BoardSL
    {
        public static readonly int BACKLOG = 0;
        public static readonly int IN_PROGRESS = 1;
        public static readonly int DONE = 2;


        public string name;
        public string owner;
        public List<string> activeUsers;
        public int ID;
        public List<TaskSL> backlog;
        public List<TaskSL> inprogress;
        public List<TaskSL> done;

        /// <summary>
        /// A consturctor for the boardSL
        /// </summary>
        /// <param name="b"></param>
        public BoardSL(BoardBL b)
        {
            this.name = b.Name;

            Dictionary<int, ColumnBL> d = b.Columns;

            this.backlog = BoardServices.ConvertTaskList(d[BACKLOG].Tasks);
            this.inprogress = BoardServices.ConvertTaskList(d[IN_PROGRESS].Tasks);
            this.done = BoardServices.ConvertTaskList(d[DONE].Tasks); //building the collumns to collumns SL

            this.ID = b.BoardID;
            this.owner = b.Owner;
            this.activeUsers = b.ActiveUsers;
        }   
    }
}
