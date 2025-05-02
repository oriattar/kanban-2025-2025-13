using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using IntroSE.Kanban.Backend.Buiesness_layer;


namespace IntroSE.Kanban.Backend.ServiceLayer
{
    internal class UserSL
    {
        private string email;
        private List<BoardSL> boards;

        public string Email { get; set; }

        public List<BoardSL> Boards { get; set; }

        /// <summary>
        /// Constructs a service-layer User object based on a business-layer UserBL.
        /// </summary>
        /// <param name="u">The UserBL business object.</param>
        /// 
        public UserSL(UserBL u)
        {
            if (u == null)
                throw new ArgumentNullException(nameof(u), "UserBL cannot be null.");

            this.Email = u.Email;
            this.Boards = new List<BoardSL>();

            foreach (var board in u.Boards)
            {
                this.Boards.Add(new BoardSL(board));
            }
        }
    }
}
