using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace IntroSE.Kanban.Backend.Buiesness_layer
{
    internal class UserFacade
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

        public UserBL Register(string email, string password) /* NOT FULLY IMPLEMENTED, PARTLY FOR TESTS */
        {
            Log.Info($"Attempting to register user with email: {email}");

            if (this.users.ContainsKey(email))
            {
                Log.Warn($"Registration failed: Email '{email}' already exists.");
                throw new ArgumentException("Email already exists");
            }

            UserBL user;

            try
            {
                user = new UserBL(email, password);
                Log.Info($"User object created successfully for email: {email}");
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to create UserBL object for email '{email}': {ex.Message}", ex);
                throw; 
            }

            this.users.Add(email, user);
            this.aFacade.Register(email);
            Log.Info($"User with email '{email}' successfully registered and added to dictionary.");

            return user;
        }


        /// <summary>
        /// Attempts to log in a user with the given email and password.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>The logged-in UserBL object.</returns>
        /// <exception cref="ArgumentException">Thrown when the email doesn't exist or the password is incorrect.</exception>
        public UserBL Login(string email, string password)
        {
            Log.Info($"Attempting login for email: {email}");

            if (!this.users.ContainsKey(email))
            {
                Log.Warn($"Login failed: Email '{email}' not found.");
                throw new ArgumentException("Email or password incorrect.");
            }

            if (!users[email].Login(password))
            {
                Log.Warn($"Login failed: Incorrect password for email '{email}'.");
                throw new ArgumentException("Email or password incorrect.");
            }

            Log.Info($"Login successful for email: {email}");
            aFacade.Login(email);
            return users[email];
        }

        public void Logout(string email)
        {
            this.aFacade.Logout(email);
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
        /// <param name="limit"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public BoardBL CreateBoard(string name, String email,int limit)
        {
            if (!this.aFacade.IsLoggedIn(email))
            {
                Log.Warn("Cant create a board while not logged in. - BOARDFACADE");
                throw new Exception("Cant create a board while not logged in.");
            }
            if (limit < 0)
            {
                Log.Warn("Limit is invalid. - BOARDFACADE");
                throw new Exception("Limit is invalid");
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
            BoardBL b = new BoardBL(name, limit);

            Log.Info("A new board:" + name +" to the user:" + email);
            u.Boards.Add(b);
            
            return b;
        }
    }
}