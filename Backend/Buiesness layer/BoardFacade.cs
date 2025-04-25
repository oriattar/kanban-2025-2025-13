using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;
using log4net;

namespace IntroSE.Kanban.Backend.Buiesness_layer
{
    public class BoardFacade
    {
        private Dictionary<string, List<BoardBL>> boards;
        private AuthenticationFacade aFacade;

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BoardFacade(AuthenticationFacade aFacade)
        {
            this.boards  = new Dictionary<string, List<BoardBL>>();
            this.aFacade = aFacade;
        }

        public string DeleteBoard(string nameboard, string email)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Registers a user's boardlist if one dosnt exist for the user in the facade
        /// </summary>
        /// <param name="b"></param>
        /// <exception cref="Exception"></exception>
        public void RegisterInFacade(UserBL user)
        {
            if (!this.boards.ContainsKey(user.Email))
            {
                this.boards.Add(user.Email,user.Boards);
                Log.Info("A list was added to the user:" + user.Email + " in BOARDFACADE");
            }
        }

        

        public void advanceTasK(string idtask, string boardname)
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
