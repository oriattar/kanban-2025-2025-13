using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend.Controllers;

namespace Frontend.Model
{
    public class UserModel
    {

        private List<BoardModel> boards;
        public List<BoardModel> Boards { get => boards; set => boards = value; }

        private UserController uc;

        public string Email { get; }

        internal UserModel(string email, UserController uc)
        {
            this.uc = uc;
            this.Email = email;
        }

    }
}
