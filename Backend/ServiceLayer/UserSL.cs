using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using IntroSE.Kanban.Backend.Buiesness_layer;
using log4net;

[assembly: InternalsVisibleTo("UnitTests")]

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    internal class UserSL
    {

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            {
                Log.Error("Attempted to create UserSL with null UserBL. -USER_SL");
                throw new ArgumentNullException(nameof(u), "UserBL cannot be null.");
            }

            Log.Info($"Creating UserSL for user with email: {u.Email} -USER_SL");

            this.Email = u.Email;
            this.Boards = new List<BoardSL>();

            foreach (var board in u.Boards)
            {
                this.Boards.Add(new BoardSL(board));
            }

            Log.Info($"UserSL created with {Boards.Count} board(s). -USER_SL");

        }
    }
}
