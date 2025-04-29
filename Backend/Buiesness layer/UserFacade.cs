using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace IntroSE.Kanban.Backend.Buiesness_layer
{
    public class UserFacade
    {
        private Dictionary<string, UserBL> users;
        private AuthenticationFacade aFacade;
        

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// UserFacade constructor, sets up the dictionary
        /// </summary>
        /// <param name="aFacade"></param>
        public UserFacade(AuthenticationFacade aFacade)
        {
            this.users = new Dictionary<string, UserBL>();
            this.aFacade = aFacade;
        }

        public UserBL Login(string email, string password)
        {
            throw new NotImplementedException();
        }

        
        public void Register(string email, UserBL user)/* NOT IMPLEMNETED FULLY, PARTLY FOR TESTS*/
        {
            this.users[email] = user;
     
        }
        /// <summary>
        /// Returns user object from the dictionary, if one exist
        /// If not exception is thrown
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public UserBL GetUserBL(string email)
        {
            if(!this.users.ContainsKey(email))
            {
                Log.Warn("Cant find user email in dictionary - USERFACADE");
                throw new Exception("Cant find user email in dictionary");
            }
            return this.users[email];
        }

        /// <summary>
        /// User facade - creating board if its not already exist in the list.
        /// If user is not logged in, board already exist by name or if user is not exist
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public void CreateBoard(string name, String email)
        {
            if (!this.aFacade.IsLoggedIn(email))
            {
                Log.Warn("Cant create a board while not logged in. - USERFACADE");
                throw new Exception("Cant create a board while not logged in.");
            }
           

            UserBL u = this.GetUserBL(email);
            List<BoardBL> userBoards = u.Boards;

            foreach (BoardBL board in userBoards) //checks whether a board is already exist
            {
                if (board.Name.Equals(name))
                {
                    Log.Warn("Board already exist on User's BoardListBL - BoardListBL");
                    throw new Exception("Board already exist on User's BoardListBL");
                }
            }
            BoardBL b = new BoardBL(name);

            Log.Info("A new board:" + name +" to the user:" + email);
            u.Boards.Add(b);
            
        }
    }
}