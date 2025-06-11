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

        public void Create_Task(string email,string boardName,string title,string desc,DateTime dueDate)
        {
            Response<string> response = JsonSerializer.Deserialize<Response<string>>(this.ts.CreateTask(email, boardName, title, desc, dueDate));

            if(response.ErrorMessage!=null)
                throw new Exception(response.ErrorMessage);
        }

        public void Advance_Task(string email,string bName,int col,int taskID)
        {
            Response<string> response = JsonSerializer.Deserialize<Response<string>>(this.ts.AdvanceTask(email,bName,col,taskID));

            if(response.ErrorMessage!=null) 
                throw new Exception(response.ErrorMessage);
        }
    }
}
