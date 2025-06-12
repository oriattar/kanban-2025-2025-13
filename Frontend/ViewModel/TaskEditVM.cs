using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Frontend.Controllers;
using Frontend.Model;

namespace Frontend.ViewModel
{
    public class TaskEditVM : Notifiable
    {
        private TaskController taskController = ControllerFactory.Instance.taskController;
        private UserModel user;
        private BoardModel board;
        private TaskModel task;

        private string editTitle;
        private string editDescription;
        private string editAssignee;
        private DateTime editDate;
        private string errorMsg;
        private bool status;

        /// <summary>
        /// Constructs the edit task window data context.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="board"></param>
        /// <param name="task"></param>
        public TaskEditVM(UserModel user, BoardModel board, TaskModel task)
        {
            this.user = user;
            this.board = board;
            this.task = task;

            this.status = false;

            this.errorMsg = string.Empty;
            this.editTitle = string.Empty;
            this.editDescription = string.Empty;
            this.editAssignee = string.Empty;
            this.editDate = DateTime.MinValue;
        }

        public UserModel User { get => user; }
        public BoardModel Board { get => board;  }
        public TaskModel Task { get => task; }
        public string EditTitle { get => editTitle; set => editTitle = value; }
        public string EditDescription { get => editDescription; set => editDescription = value; }
        public string EditAssignee { get => editAssignee; set => editAssignee = value; }
        public DateTime EditDate { get => editDate; set => editDate = value; }
        public string ErrorMsg
        {
            get => errorMsg; set
            {
                errorMsg = value;
                RaisePropertyChanged("ErrorMsg");
            }

        }
        public bool Status { get => status; set
            {
                status = value;
                RaisePropertyChanged("Status");
            }
        }

        /// <summary>
        /// Operates update title serivce
        /// </summary>
        public void Edit_Title()
        {
            try
            {
                this.taskController.Edit_title(this.user.Email, this.board.BName, this.task.ColID, this.task.TaskID, this.EditTitle);
                ErrorMsg = "Edit was successful.";
                Status = true; //colors green
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
                Status = false; // colors red
            }
        }

        /// <summary>
        /// Operates update description serivce
        /// </summary>
        public void Edit_Description()
        {
            try
            {
                this.taskController.Edit_desc(this.user.Email, this.board.BName, this.task.ColID, this.task.TaskID, this.EditDescription);
                ErrorMsg = "Edit was successful.";
                Status = true;
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
                Status = false;
            }
        }

        /// <summary>
        /// Operates task assignment service.
        /// </summary>
        public void Edit_Assignee()
        {
            try
            {
                this.taskController.Edit_assignee(this.user.Email, this.board.BName, this.task.ColID, this.task.TaskID, this.EditAssignee);
                ErrorMsg = "User " + EditAssignee + " was assigned to the task.";
                Status = true;
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
                Status = false;
            }
        }

        /// <summary>
        /// using the deadline update service from past milestones.
        /// </summary>
        public void Edit_DueDate()
        {
            try
            {
                this.taskController.Edit_duedate(this.user.Email, this.board.BName, this.task.ColID, this.task.TaskID, this.EditDate);
                ErrorMsg = "Edit was successful.";
                Status = true;
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
                Status = false;
            }
        }


    }
}
