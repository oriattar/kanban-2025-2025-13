using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class UserModel
    {
        private string email;

        private List<BoardModel> boards;
        public string Email { get => email; set => email = value; }
        public List<BoardModel> Boards { get => boards; set => boards = value; }
    }
}
