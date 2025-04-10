using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.Backend.Buiesness_layer
{
    public class BoardListBL
    {
        private Dictionary<string, BoardBL> boards;
        private AuthenticationFacade aFacade;

        public BoardListBL(Dictionary<string, BoardBL> boards, AuthenticationFacade aFacade)
        {
            this.boards = boards;
            this.aFacade = aFacade;
        }

        public Dictionary<string, BoardBL> Boards { get => boards; set => boards = value; }
        internal AuthenticationFacade AFacade { get => aFacade; set => aFacade = value; }

        public string DeleteBoard(string nameboard, string email)
        {
            throw new NotImplementedException();
        }


        public void advanceTasl(string idtask, string boardname)
        {
            throw new NotImplementedException();
        }

        public TaskBL CreateTask(DateTime deadlinem, string title, string description, string bname)
        {
            throw new NotImplementedException();
        }

        public string Setlimit(int limit)
        {
            throw new NotImplementedException();
        }

        public TaskSL EditTask(DateTime deadline, string title, string description, string nboard, string idtask)
        {
            throw new NotImplementedException();
        }

        public BoardBL GetBoard(string name)
        {
            throw new NotImplementedException();
        }

        public List<TaskBL> GetInProgressList()
        {
            throw new NotImplementedException();
        }

    }
}
