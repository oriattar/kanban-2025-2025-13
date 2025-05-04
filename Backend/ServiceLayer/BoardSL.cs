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
        public string name;

        public List<TaskSL> backlog;
        public List<TaskSL> inprogress;
        public List<TaskSL> done;
        public long nextID;

        /// <summary>
        /// A consturctor for the boardSL
        /// </summary>
        /// <param name="b"></param>
        public BoardSL(BoardBL b)
        {
            this.name = b.Name;

            Dictionary<int, ColumnBL> d = b.Columns;

            this.backlog = BoardFacade.ConvertTaskList(d[0].Tasks);
            this.inprogress = BoardFacade.ConvertTaskList(d[1].Tasks);
            this.done = BoardFacade.ConvertTaskList(d[2].Tasks); //building the collumns to collumns SL
            this.nextID = b.NextTaskId;
        }   
    }
}
