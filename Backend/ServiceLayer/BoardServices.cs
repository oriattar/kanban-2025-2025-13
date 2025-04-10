using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buiesness_layer;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class BoardServices
    {
        private UserFacade uFacade;

        public BoardServices(UserFacade uFacade)
        {
            this.uFacade = uFacade;
        }

        public UserFacade UFacade { get => uFacade; set => uFacade = value; }

        public Response<BoardSL> CreateBoard(string email, string boardName)
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




        public Response<List<TaskSL>> GetInProgressList(string email)
        {
            throw new NotImplementedException();
        }


    }


}
