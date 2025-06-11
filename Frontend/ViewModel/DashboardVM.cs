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
        private UserModel user;
        private string boardCreationName;
        private string errorMsg;
        private string userEmail;

        private bool status;

        private ObservableCollection<BoardModel> boards;


        /// <summary>
        /// Constructor for the data context - init the fields for the properites
        /// </summary>
        /// <param name="u"></param>
        public DashboardVM(UserModel u) {
        
            this.user = u;
            this.boards = new ObservableCollection<BoardModel>();
            this.BoardCreationName = string.Empty;
            this.errorMsg = string.Empty;
            this.userEmail = u.Email;
            
            LoadUserBoards();
        }

        
        /// <summary>
        /// A method that loads all of the user boards in order to display in the item control
        /// </summary>
        public void LoadUserBoards()
        {
            try
            {
                this.boards = this.boardController.GetBoards(this.User.Email); // getting the boards
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
                Status = false; //colors the error msg red
            }
            
        }

        /*
         * Getters and setters
         */

        public string UserEmail { get => userEmail; set => userEmail = value; }
        public string BoardCreationName { get => boardCreationName; set => boardCreationName = value; }
        public UserModel User { get => user;}
        public ObservableCollection<BoardModel> Boards { get => boards; }
        public string ErrorMsg { get => errorMsg; set
            {
                errorMsg = value; 
                RaisePropertyChanged("ErrorMsg"); //Since the binding mode is one way, we need to notify the gui uppon updating the field
            }
        }

        public bool Status
        {
            get => status; set
            {
                this.status = value;
                RaisePropertyChanged("Status");
            }
        }

        

        /// <summary>
        /// A method operates the board creation operation
        /// Using the board controller.
        /// </summary>
        public void CreateBoard()
        {
            if (string.IsNullOrEmpty(boardCreationName)) //if the text box is empty, updating from gui to source
            {
                ErrorMsg = "Cant create a board without a name.";
                Status = false;
            }
            else
            {
                try
                {
                    this.boardController.CreateBoard(this.boardCreationName,this.User.Email);

                    this.boards.Add(new BoardModel(this.boardCreationName, this.User.Email)); //adds the new board to an observable colletion, so the gui updates on the spot.
                    ErrorMsg = "Board Created Successfully";
                    Status = true; //colors the msg green
                }
                catch (Exception ex)
                {
                    ErrorMsg = ex.Message;
                    Status = false;
                }
            }
        }

        /// <summary>
        /// A method responsible for the board deltion
        /// using the board controller.
        /// </summary>
        /// <param name="bName"></param>
        public void DeleteBoard(string bName)
        {
            try
            {
                this.boardController.DeleteBoard(this.User.Email, bName); //removing at backend
                this.boards.Remove(this.FindBoard(bName)); //looking for the board and removing it from the list
                ErrorMsg = "Board deleted successfully.";
                Status = true;
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
                Status = false;
            }
        }

        /// <summary>
        /// Helper method in the datacontext, looks for a board by its name in the observable collection
        /// </summary>
        /// <param name="bName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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
