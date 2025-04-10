using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using IntroSE.Kanban.Backend.Buiesness_layer;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class TaskServices
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

        public Response<TaskSL> EditTask(DateTime newDeadline, string newTitle, string newDescription, long idtask, string bName,string email)
        {
            throw new NotImplementedException();
        }


        public Response<string> AdvanceTask(string email, string boardName, long idtask)
        {
            throw new NotImplementedException();
        }



    }
}
