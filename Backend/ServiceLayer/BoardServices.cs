using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class BoardServices
    {
        private UserFacade uFacade;

        public BoardServices(UserFacade uFacade)
        {
            this.uFacade = uFacade;
        }

        public UserFacade UFacade { get => uFacade; set => uFacade = value; }

        public Response<BoardSL> CreateBourd(string email, string boardName)
        {
            throw new NotImplementedException();
        }

        public Response<string> DeleteBoard(string email, string boardName)
        {
            throw new NotImplementedException();
        }

        public Response<string> SetLimit(string email, string boardName, int limit)
        {
            throw new NotImplementedException();
        }




        public Response<List<TaskSL>> GetInProgressList()
        {
            throw new NotImplementedException();
        }


    }


}
