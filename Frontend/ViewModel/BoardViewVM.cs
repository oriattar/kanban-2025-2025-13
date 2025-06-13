using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
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
        private ComboBoxItem selectedCol;
        private string limitText;

        private string backlog_limit;
        private string inprogress_limit;
        private string done_limit;

        ObservableCollection<TaskModel> backlog;
        ObservableCollection<TaskModel> inprogress;
        ObservableCollection<TaskModel> done;

        /// <summary>
        /// Constructs the required properties for the window.
        /// </summary>
        /// <param name="u"></param>
        /// <param name="board"></param>
        public BoardViewVM(UserModel u,BoardModel board)
        {
            this.user = u;
            this.board = board;
            this.ErrorMsg = string.Empty;
            this.boardName = board.BName;
            this.errorMsg = string.Empty;
            
            this.limitText = string.Empty;
            this.selectedCol = new ComboBoxItem();

            this.backlog_limit = string.Empty;
            this.done_limit = string.Empty;
            this.inprogress_limit = string.Empty; //limit propertios

            this.backlog = new ObservableCollection<TaskModel>();
            this.inprogress = new ObservableCollection<TaskModel>();
            this.done = new ObservableCollection<TaskModel>(); //One task list for each col.
            
            this.LoadTasks(); // loads the tasks from backend
            this.LoadLimits(); //loads columns limits
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
        public ComboBoxItem SelectedCol { get => selectedCol; set => selectedCol = value; } //droplist item
        public string LimitText { get => limitText; set => limitText = value; }
        public string Backlog_limit { get => backlog_limit; set
            { 
                backlog_limit = value;
                RaisePropertyChanged("Backlog_limit");
            }
        }
        public string Inprogress_limit { get => inprogress_limit; set
            {
                inprogress_limit = value;
                RaisePropertyChanged("Inprogress_limit");
            }
        }
        public string Done_limit { get => done_limit; set
            {
                done_limit = value;
                RaisePropertyChanged("Done_limit");
            }
        }


        /// <summary>
        /// Loads all current board tasks from backend
        /// </summary>
        private void LoadTasks()
        {
            try
            {
                this.backlog = this.boardController.GetTasks(user.Email, this.boardName, BACKLOG);
                this.inprogress = this.boardController.GetTasks(user.Email, this.boardName, IN_PROGRESS);
                this.done = this.boardController.GetTasks(user.Email, this.boardName, DONE); //loads each column, using the service from past milestone
            }
            catch (Exception ex)
            {

                this.errorMsg = ex.Message;
            }
        }

        /// <summary>
        /// Loads the columns limits, using the service from past milestone.
        /// </summary>
        private void LoadLimits()
        {
            try
            {
               
                this.Backlog_limit = this.LoadLim(User.Email,boardName,BACKLOG);
                this.Inprogress_limit = this.LoadLim(User.Email, boardName, IN_PROGRESS);
                this.Done_limit = this.LoadLim(User.Email, boardName, DONE);
            }
            catch (Exception ex)
            {

                this.errorMsg = ex.Message;
            }
        }

        /// <summary>
        /// Helper method that read the limit of the input column from backend
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private string LoadLim(string email,string boardName,int col)
        {
            int limit = this.boardController.GetLimit(user.Email, this.boardName, col);
            if (limit == -1)
                return string.Empty;

            return limit.ToString();
        }

        /// <summary>
        /// Method that operates the task transfer.
        /// </summary>
        /// <param name="task"></param>
        public void AdvanceTask(TaskModel task)
        {
            try
            {
               this.taskController.Advance_Task(this.user.Email,this.boardName,task.ColID,task.TaskID);
               this.TransferTask(task,task.ColID); //updates the observable collection for imidate update
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
            }
        }

        /// <summary>
        /// A helper method to advance the task internaly in the GUI
        /// </summary>
        /// <param name="task"></param>
        /// <param name="sourceCol"></param>
        /// <exception cref="Exception"></exception>
        private void TransferTask(TaskModel task,int sourceCol)
        {
            if (sourceCol == BACKLOG)
            {
                this.inprogress.Add(task);
                this.backlog.Remove(task);
                task.ColID = IN_PROGRESS;
            }
            else if (sourceCol == IN_PROGRESS)
            {
                this.done.Add(task);
                this.inprogress.Remove(task);
                task.ColID = DONE;
            }
            else
                throw new Exception("Invalid column to transfer from.");
        }

        /// <summary>
        /// Operates the column limitation operation, using past service
        /// </summary>
        public void LimitCol()
        {
            try
            {
                if (string.IsNullOrEmpty(limitText))
                    ErrorMsg = "Please insert selected limit.";
                else
                {
                    this.boardController.Limit_Col(this.user.Email, this.boardName, int.Parse(this.LimitText), ConvertColName(SelectedCol.Content.ToString()));
                    this.LoadLimits();
                    ErrorMsg = string.Empty;
                }
            }
            catch (Exception e)
            {
                ErrorMsg= e.Message;
            }
        }

        /// <summary>
        /// Helper method that converts the column name to its id.
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        private int ConvertColName(string colName)
        {
            if (colName.Equals("Backlog"))
                return BACKLOG;
            else if (colName.Equals("In Progress"))
                return IN_PROGRESS;
            else
                return DONE;
        }
    }
}
