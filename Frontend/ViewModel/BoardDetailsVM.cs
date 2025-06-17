using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend.Model;
using Frontend.Controllers;

namespace Frontend.ViewModel
{
    public class BoardDetailsVM :Notifiable
    {
        private BoardController boardController = ControllerFactory.Instance.boardController;
        private UserModel user;
        private BoardModel board;
        private string boardName;
        private string errorMsg;
        private string boardOwner;
        private bool status;

        private string addMemberText;
        private string changeOwnerText;

        private ObservableCollection<UserModel> activeUsers;

        /// <summary>
        /// Constructs the data context
        /// </summary>
        /// <param name="user"></param>
        /// <param name="board"></param>
        public BoardDetailsVM(UserModel user, BoardModel board)
        {
            this.user = user;
            this.board = board;
            this.boardName = board.BName;
            this.errorMsg = string.Empty;
            this.activeUsers = this.LoadActiveUsers();
            this.boardOwner = board.Owner;

            this.addMemberText = string.Empty;
            this.changeOwnerText = string.Empty;
        }

        public UserModel User { get => user; set => user = value; }
        public BoardModel Board { get => board; set => board = value; }
        public string BoardName { get => boardName; set => boardName = value; }
        public string ErrorMsg { get => errorMsg; set
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
        public ObservableCollection<UserModel> ActiveUsers { get => activeUsers; set => activeUsers = value; }
        public string BoardOwner { get => boardOwner; set
            {
                boardOwner = value;
                RaisePropertyChanged("BoardOwner");
            }
        }
        public string AddMemberText { get => addMemberText; set => addMemberText = value; }
        public string ChangeOwnerText { get => changeOwnerText; set => changeOwnerText = value; }

        /// <summary>
        /// Method responsible of loading the active users using the controller
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<UserModel> LoadActiveUsers()
        {
            ObservableCollection<UserModel> res = null;
            try
            {
                res = this.boardController.Get_ActiveMemebers(user.Email, board.BName);
            }
            catch (Exception ex)
            {
                this.ErrorMsg = ex.Message;
                this.Status = false;
            }
            return res;
        }

        /// <summary>
        /// Method responsible of adding the input user from the text box, using the board controller
        /// </summary>
        public void AddMember()
        {
            try
            {
                this.boardController.JoinUser(this.AddMemberText, this.board.Id);
                this.activeUsers.Add(new UserModel(this.AddMemberText));
                this.ErrorMsg = "User joined successfuly.";
                this.Status = true;
            }
            catch (Exception e)
            {
                this.ErrorMsg = e.Message;
                this.Status = false;
            }
        }

        /// <summary>
        /// Operates the board leave operation.
        /// </summary>
        public void LeaveBoard()
        {
            try
            {
                this.boardController.Leave_Board(this.user.Email, this.board.Id);
            }
            catch (Exception e)
            {
                this.ErrorMsg = e.Message;
                this.Status = false;
                throw;

            }
        }

        /// <summary>
        /// Method resposible for owner changing, using the controller
        /// </summary>
        public void ChangeOwner()
        {
            try
            {
                if (!this.user.Email.Equals(board.Owner))
                    throw new Exception("Cant promote a user while not beeing the owner.");
                this.boardController.Change_Owner(this.board.Owner, this.ChangeOwnerText, this.board.BName);
                this.BoardOwner = this.ChangeOwnerText; // if owner change was successfull, update at the GUI
                this.board.Owner = this.ChangeOwnerText;
                this.ErrorMsg = "Owner promoted successfuly";
                this.Status = true;
            }
            catch (Exception e)
            {

                this.ErrorMsg = e.Message;
                this.Status = false;
            }
        }
        
    }
}
