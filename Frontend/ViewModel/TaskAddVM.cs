using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend.Controllers;
using Frontend.Model;

namespace Frontend.ViewModel
{
    public class TaskAddVM : Notifiable
    {
        private TaskController taskController = ControllerFactory.Instance.taskController;
        private UserModel user;
        private BoardModel board;

        private string title;
        private string description;
        private DateTime dueDate;

        private string errorMsg;

        public TaskAddVM(UserModel user, BoardModel board)
        {
            this.user = user;
            this.board = board;
            this.title = string.Empty;
            this.description = string.Empty;
            this.dueDate = DateTime.MinValue;

            this.errorMsg = string.Empty;
        }

        public UserModel User { get => user; set => user = value; }
        public BoardModel Board { get => board; set => board = value; }
        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }
        public DateTime DueDate { get => dueDate; set => dueDate = value; }
        public string ErrorMsg { get => errorMsg; 
            set
            {
                errorMsg = value;
                RaisePropertyChanged("ErrorMsg");
            }
        }

        public void CreateTask()
        {
            try
            {
                this.taskController.Create_Task(this.user.Email, this.board.BName, this.Title, this.Description, this.DueDate);
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
                throw;
            }
        }


    }
}
