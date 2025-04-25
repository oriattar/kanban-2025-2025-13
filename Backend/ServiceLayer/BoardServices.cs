using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buiesness_layer;
using log4net;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class BoardServices
    {
        private UserFacade uFacade;
        private BoardFacade bFacade;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BoardServices(UserFacade uFacade, BoardFacade bFacade)
        {
            this.uFacade = uFacade;
            this.bFacade = bFacade;
        }
        /// <summary>
        /// Creates a new board and returns response<BoardSL> if opeartion was succefull, else response will only contain error msg.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public Response<BoardSL> CreateBoard(string email, string boardName, int limit)
        {
            Response<BoardSL> res = null;
            try
            {
                bFacade.RegisterInFacade(uFacade.GetUserBL(email)); // adds the list in the board facade if user isnt registerd
                BoardBL board = this.uFacade.CreateBoard(boardName, email,limit); // creates the board
                res = new Response<BoardSL>(null, new BoardSL(board));
            }
            catch (Exception ex)
            {
                res = new Response<BoardSL>(ex.Message,null);
                Log.Error("Error in creating board BOARDSERVICES:" + ex.Message);
            }
            return res;
        }

        public Response<string> DeleteBoard(string email, string boardName)
        {
            throw new NotImplementedException();
        }

        public Response<string> SetLimit(string email, string boardName, int limit)
        {
            throw new NotImplementedException();
        }

        public Response<List<TaskSL>> GetInProgressList(string email)
        {
            throw new NotImplementedException();
        }


    }


}
