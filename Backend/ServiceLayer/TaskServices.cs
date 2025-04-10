using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class TaskServices
    {
        private UserFacade uFacade;

        public TaskServices(UserFacade uFacade)
        {
            this.uFacade = uFacade;
        }

        public UserFacade UFacade { get => uFacade; set => uFacade = value; }




        public Response<TaskSL> CreateTask(DateTime deadline,string title,string description,string bName,string email)
        {
            throw new NotImplementedException();
        }

        public Response<TaskSL> EditTask(DateTime newDeadline, string newTitle, string newDescription, string bName, string idtask)
        {
            throw new NotImplementedException();
        }


        public void AdvanceTask(string email, string boardName, string idtask)
        {
            throw new NotImplementedException();
        }



    }
}
