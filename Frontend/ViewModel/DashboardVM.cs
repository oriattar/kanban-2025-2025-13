using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend.Model;

namespace Frontend.ViewModel
{
    public class DashboardVM
    {
        private string userEmail;
        
        public DashboardVM(UserModel u) {
        
            this.userEmail = u.Email;
            this.boards = u.Boards;
        }

        private List<BoardModel> boards;

        public string UserEmail { get => userEmail;}
        public List<BoardModel> Boards { get => boards; }
    }
}
