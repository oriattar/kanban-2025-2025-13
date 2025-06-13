using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;
using Microsoft.VisualBasic;

namespace Frontend.Controllers
{
    public class TaskController
    {
        private TaskServices ts;

        public TaskController(TaskServices ts)
        {
            this.ts = ts;
        }

        /// <summary>
        /// Creating task, using service from backend
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="title"></param>
        /// <param name="desc"></param>
        /// <param name="dueDate"></param>
        /// <exception cref="Exception"></exception>
        public void Create_Task(string email,string boardName,string title,string desc,DateTime dueDate)
        {
            Response<string> response = JsonSerializer.Deserialize<Response<string>>(this.ts.CreateTask(email, boardName, title, desc, dueDate));

            if(response.ErrorMessage!=null)
                throw new Exception(response.ErrorMessage);
        }

        /// <summary>
        /// Advancing a task using the service.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="bName"></param>
        /// <param name="col"></param>
        /// <param name="taskID"></param>
        /// <exception cref="Exception"></exception>
        public void Advance_Task(string email,string bName,int col,int taskID)
        {
            Response<string> response = JsonSerializer.Deserialize<Response<string>>(this.ts.AdvanceTask(email,bName,col,taskID));

            if(response.ErrorMessage!=null) 
                throw new Exception(response.ErrorMessage);
        }

        /*
         * 
         * Following methods are for task properties updating, using the corresponding services.
         * 
         */
        public void Edit_title(string email, string bName, int col, int taskID, string title)
        {
            Response<string> response = JsonSerializer.Deserialize<Response<string>>(this.ts.UpdateTaskTitle(email,bName,col,taskID,title));

            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
        }

        public void Edit_desc(string email, string bName, int col, int taskID, string desc)
        {
            Response<string> response = JsonSerializer.Deserialize<Response<string>>(this.ts.UpdateTaskDescription(email, bName, col, taskID, desc));

            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
        }

        public void Edit_duedate(string email, string bName, int col, int taskID, DateTime dueDate)
        {
            Response<string> response = JsonSerializer.Deserialize<Response<string>>(this.ts.UpdateTaskDeadline(email, bName, col, taskID, dueDate));

            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
        }

        public void Edit_assignee(string email, string bName, int col, int taskID, string assignee)
        {
            Response<string> response = JsonSerializer.Deserialize<Response<string>>(this.ts.AssignTask(email,bName,col,taskID,assignee));

            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
        }
    }
}
