using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using IntroSE.Kanban.Backend.Buiesness_layer;
using log4net;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class TaskServices
    {
        private BoardFacade bFacade;

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Constructor to the TaskServices
        /// </summary>
        /// <param name="uFacade"></param>
        /// <param name="boardFacade"></param>
        public TaskServices(BoardFacade boardFacade)
        {
            this.bFacade = boardFacade;

        }


        /// <summary>
        /// Operates the task creation service
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="dueDate"></param>
        /// <returns></returns>
        public string CreateTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            Response<string> response = null;
            try
            {
                this.bFacade.CreateTask(email, boardName, title, description, dueDate);
                response = new Response<string>(null, null);
                
            }
            catch (Exception ex)
            {
                response = new Response<string>(ex.Message,null);
                Log.Error("Task creation faild due to - " + ex.Message);
            }

            return JsonSerializer.Serialize(response);
        }

 
        /// <summary>
        /// Operates the task advancing service
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="col"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public string AdvanceTask(string email, string boardName, int col, int taskId)
        {
            Response<string> response = null;
            try
            {
                this.bFacade.AdvanceTasK(email, boardName, col, taskId);
                response = new Response<string>(null,null);
            }
            catch (Exception ex)
            {
                response = new Response<string>(ex.Message,null);
                Log.Error("Failed advancing task due to - " + ex.Message);
            }
            return JsonSerializer.Serialize(response);
        }

        /// <summary>
        /// Operates editing task title.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="col"></param>
        /// <param name="taskId"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public string UpdateTaskTitle(string email, string boardName, int col, int taskId, string title)
        {
            Response<string> response = null;
            try
            {
                this.bFacade.EditTask(email, boardName, col, taskId, DateTime.MinValue, title, null,BoardFacade.TITLE);
                response = new Response<string>(null, null);
            }
            catch (Exception ex)
            {
                response = new Response<string>(ex.Message, null);
                Log.Error("Failed editing title due to - " + ex.Message);
            }
            return JsonSerializer.Serialize(response);
        }

        /// <summary>
        /// Operates update task decription operations
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="col"></param>
        /// <param name="taskId"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public string UpdateTaskDescription(string email, string boardName, int col, int taskId, string desc)
        {
            Response<string> response = null;
            try
            {   
                this.bFacade.EditTask(email, boardName, col, taskId, DateTime.MinValue, null,desc,BoardFacade.DESCRIPTION);
                response = new Response<string>(null, null);
            }
            catch (Exception ex)
            {
                response = new Response<string>(ex.Message, null);
                Log.Error("Failed editing description due to - " + ex.Message);
            }
            return JsonSerializer.Serialize(response);
        }

        /// <summary>
        /// Operates the update task deadline operation
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="col"></param>
        /// <param name="taskId"></param>
        /// <param name="deadline"></param>
        /// <returns></returns>
        public string UpdateTaskDeadline(string email, string boardName, int col, int taskId, DateTime deadline)
        {
            Response<string> response = null;
            try
            {
                this.bFacade.EditTask(email, boardName, col, taskId, deadline, null, null, BoardFacade.DUEDATE);
                response = new Response<string>(null, null);
            }
            catch (Exception ex)
            {
                response = new Response<string>(ex.Message, null);
                Log.Error("Failed editing duetime due to - " + ex.Message);
            }
            return JsonSerializer.Serialize(response);

        }

        /// <summary>
        /// Operates the task assignment service
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskID"></param>
        /// <param name="emailAssignee"></param>
        /// <returns></returns>
        public string AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            Response<string> response = null;
            try
            {
                this.bFacade.AssignTaskToMember(email, boardName, columnOrdinal, taskID, emailAssignee);
                response = new Response<string>(null, null);
            }
            catch (Exception ex)
            {
                response = new Response<string>(ex.Message, null);
                Log.Error("Failed assigning task due to - " + ex.Message);
            }
            return JsonSerializer.Serialize(response);
        }
    }
}
