using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend.Controllers;
using Frontend.Model;

namespace Frontend.ViewModel
{
    public class DashboardVM : Notifiable
    {
        BoardController boardController = ControllerFactory.Instance.boardController;
        private string userEmail;
        private string boardCreationName;
        private string errorMsg;

        private ObservableCollection<BoardModel> boards;


        public DashboardVM(UserModel u) {
        
            this.userEmail = u.Email;
            this.boards = new ObservableCollection<BoardModel>();
            this.BoardCreationName = string.Empty;
            this.errorMsg = string.Empty;

            LoadUserBoards();
        }

        
        public void LoadUserBoards()
        {
            try
            {
                this.boards = this.boardController.GetBoards(this.UserEmail);
            }
            catch (Exception ex)
            {
                ErrorMsg = string.Empty;
            }
            
        }

        public string BoardCreationName { get => boardCreationName; set => boardCreationName = value; }
        public string UserEmail { get => userEmail;}
        public ObservableCollection<BoardModel> Boards { get => boards; }
        public string ErrorMsg { get => errorMsg; set
            {
                errorMsg = value;
                RaisePropertyChanged("ErrorMsg");
            }
        }

        public void CreateBoard()
        {
            if (string.IsNullOrEmpty(boardCreationName))
            {
                ErrorMsg = "Cant create a board without a name.";
            }
            else
            {
                try
                {
                    this.boardController.CreateBoard(this.boardCreationName,this.userEmail);
                    this.boards.Add(new BoardModel(this.boardCreationName, this.userEmail));
                    ErrorMsg = "Board Created Successfully";
                }
                catch (Exception ex)
                {
                    ErrorMsg = ex.Message;
                }
            }
        }

        public void DeleteBoard(string bName)
        {
            try
            {
                this.boardController.DeleteBoard(userEmail, bName);
                this.boards.Remove(this.FindBoard(bName));
                ErrorMsg = "Board deleted successfully.";
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
            }
        }

        private BoardModel FindBoard(string bName)
        {
            foreach (BoardModel board in this.boards)
            {
                if(board.BName.Equals(bName))
                    return board;
            }

            throw new Exception("Target board was not found in list");
        }

    }
}
