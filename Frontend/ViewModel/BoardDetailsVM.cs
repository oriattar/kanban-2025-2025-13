using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend.Model;

namespace Frontend.ViewModel
{
    public class BoardDetailsVM
    {
        private UserModel user;
        private BoardModel board;
        private string boardName;
        private string errorMsg;

        public BoardDetailsVM(UserModel user, BoardModel board)
        {
            this.user = user;
            this.board = board;
            this.boardName = board.BName;
            this.errorMsg = string.Empty;
        }

        public UserModel User { get => user; set => user = value; }
        public BoardModel Board { get => board; set => board = value; }
        public string BoardName { get => boardName; set => boardName = value; }
        public string ErrorMsg { get => errorMsg; set => errorMsg = value; }
    }
}
