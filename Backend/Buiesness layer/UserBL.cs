using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.Backend.Buiesness_layer
{
    public class UserBL
    {
        private string email;
        private string password;
        private List<BoardSL> boards;

        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        internal List<BoardSL> Boards { get => boards; set => boards = value; }

        public UserBL(string email, string password, List<BoardSL> boards)
        {
            this.email = email;
            this.password = password;
            this.boards = boards;
        }

        public bool Login (string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
