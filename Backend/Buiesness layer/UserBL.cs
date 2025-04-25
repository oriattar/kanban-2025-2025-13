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
        private List<BoardBL> boards;

        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }

        public List<BoardBL> Boards { get => boards; set => boards = value; }
       
        /// <summary>
        /// UserBL object constructor
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public UserBL(string email, string password)
        {
            this.email = email;
            this.password = password;
            this.boards = new List<BoardBL>();
        }

        public bool Login (string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
