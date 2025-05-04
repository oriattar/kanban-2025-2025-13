using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using IntroSE.Kanban.Backend.Buiesness_layer;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    internal class TaskServices
    {
        private UserFacade uFacade;
        private BoardFacade bFacade;

        /// <summary>
        /// Constructor to the TaskServices
        /// </summary>
        /// <param name="uFacade"></param>
        /// <param name="boardFacade"></param>
        public TaskServices(UserFacade uFacade, BoardFacade boardFacade)
        {
            this.uFacade = uFacade;
            this.bFacade = boardFacade;

        }

        public string CreateTask(DateTime deadline,string title,string description,string bName,string email)
        {
            throw new NotImplementedException();
        }


        public string AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            throw new NotImplementedException();
        }

        public string UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            throw new NotImplementedException();
        }
        public string UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string desc)
        {
            throw new NotImplementedException();
        }
        public string UpdateTaskDeadline(string email, string boardName, int columnOrdinal, int taskId, DateTime deadline)
        {
            throw new NotImplementedException();
        }


    }
}
