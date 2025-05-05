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

        public Response<TaskSL> CreateTask(DateTime deadline, string title, string description, string bName, string email)
        {
            try
            {
                BoardBL board = uFacade.CreateBoard(bName, email);
                TaskBL task = board.CreateTask(deadline, title, description);
                TaskSL exposedTask = new TaskSL(task.Id, task.Creationtime, task.Deadline, task.Title, task.Description);
                return new Response<TaskSL>(null, exposedTask);
            }
            catch (Exception ex)
            {
                return new Response<TaskSL>(ex.Message, default(TaskSL));
            }
        }

        public Response<TaskSL> EditTask(DateTime newDeadline, string newTitle, string newDescription, long idtask, string bName, string email)
        {
            try
            {
                BoardBL board = uFacade.CreateBoard(bName, email);
                TaskBL updatedTask = board.EditTask(newDeadline, newTitle, newDescription);
                TaskSL result = new TaskSL(updatedTask.Id, updatedTask.Creationtime, updatedTask.Deadline, updatedTask.Title, updatedTask.Description);
                return new Response<TaskSL>(null, result);
            }
            catch (Exception ex)
            {
                return new Response<TaskSL>(ex.Message, default(TaskSL));
            }
        }

        public Response<string> AdvanceTask(string email, string boardName, long idtask)
        {
            try
            {
                BoardBL board = uFacade.CreateBoard(boardName, email);
                board.AdvanceTask(idtask.ToString());
                return new Response<string>(null, "Task advanced successfully.");
            }
            catch (Exception ex)
            {
                return new Response<string>(ex.Message, null);
            }
        }
    }
}

