using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend.Controllers;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Frontend.Model
{
    public class UserModel
    {

        private List<BoardModel> boards;
        public List<BoardModel> Boards { get => boards; set => boards = value; }


        public string Email { get; }

        internal UserModel(string email)
        {
            this.Email = email;
        }

    }
}
