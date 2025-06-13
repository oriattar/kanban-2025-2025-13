using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend.Controllers;
using Frontend.Model;


namespace Frontend.ViewModel
{

    public class BoardViewVM : Notifiable
    {
        
        BoardController boardController = ControllerFactory.Instance.boardController;
        private UserModel user;
        private BoardModel board;

        private string errorMsg;
        private string boardName;


        public BoardViewVM(UserModel u,BoardModel board)
        {
            this.user = u;
            this.board = board;
            this.ErrorMsg = string.Empty;
            this.boardName = board.BName;
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
    }
}
