using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend.Controllers;
using Frontend.Model;


namespace Frontend.ViewModel
{

    public class BoardViewVM : Notifiable
    {
        public static readonly int BACKLOG = 0;
        public static readonly int IN_PROGRESS = 1;
        public static readonly int DONE = 2;

        private BoardController boardController = ControllerFactory.Instance.boardController;
        private TaskController taskController = ControllerFactory.Instance.taskController;
        private UserModel user;
        private BoardModel board;

        private string errorMsg;
        private string boardName;

        ObservableCollection<TaskModel> backlog;
        ObservableCollection<TaskModel> inprogress;
        ObservableCollection<TaskModel> done;

        public BoardViewVM(UserModel u,BoardModel board)
        {
            this.user = u;
            this.board = board;
            this.ErrorMsg = string.Empty;
            this.boardName = board.BName;

            this.backlog = new ObservableCollection<TaskModel>();
            this.inprogress = new ObservableCollection<TaskModel>();
            this.done = new ObservableCollection<TaskModel>();
            
            this.LoadTasks();
        }
        public string ErrorMsg
        {
            get => errorMsg; set
            {
                errorMsg = value;
                RaisePropertyChanged("ErrorMsg");
            }
        }

        public string BoardName { get => boardName; }
        public UserModel User { get => user; }
        public ObservableCollection<TaskModel> Backlog { get => backlog; }
        public ObservableCollection<TaskModel> Inprogress { get => inprogress;  }
        public ObservableCollection<TaskModel> Done { get => done;  }
        public BoardModel Board { get => board; set => board = value; }

        /// <summary>
        /// Loads all current board tasks from backend
        /// </summary>
        private void LoadTasks()
        {
            try
            {
                this.backlog = this.boardController.GetTasks(user.Email, this.boardName, BACKLOG);
                this.inprogress = this.boardController.GetTasks(user.Email, this.boardName, IN_PROGRESS);
                this.done = this.boardController.GetTasks(user.Email, this.boardName, DONE);
            }
            catch (Exception ex)
            {

                this.errorMsg = ex.Message;
            }
        }

        private void AdvanceTask(TaskModel task)
        {
            try
            {
                this.taskController.Advance_Task(user.Email, this.boardName)
            }
            catch (Exception ex)
            {

            }
        }
    }
}
