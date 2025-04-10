using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using IntroSE.Kanban.Backend.Buiesness_layer;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class UserSL
    {
        private string email;
        private List<BoardSL> boards;

        public string Email { get => email; set => email = value; }
        public List<BoardSL> Boards { get => boards; set => boards = value; }

        public UserSL(UserBL u)
        {
            throw new NotImplementedException();
        }

        
        
    }
}
