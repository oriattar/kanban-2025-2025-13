using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class UserSL
    {
        public string email;
        public List<BoardSL> boards;

        public UserSL() { }

        public string Email
        {
            get { return this.email; }
            set { this.email = value; }
        }
        public List<BoardSL> Boards
        {
            get { return this.boards; }
            set { this.boards = value; }
        }
    }
}
