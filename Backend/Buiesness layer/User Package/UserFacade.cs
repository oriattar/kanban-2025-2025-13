using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;

[assembly: InternalsVisibleTo("UnitTests")]


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
            this.LoadAllUsers();
        }



        /// <summary>
        /// Loads all users from the database into the application's memory.
        /// Retrieves users using the data access layer and stores them in the internal dictionary.
        /// </summary>
        public void LoadAllUsers()
        {
            Log.Info("Starting LoadAllUsers - retrieving users from database.-USER_FACADE");

            try
            {
                List<UserDAL> usersFromDb = new UserController().SelectAll();
                Log.Info($"Retrieved {usersFromDb.Count} users from database.-USER_FACADE");

                foreach (var uDAL in usersFromDb)
                {
                    var uBL = new UserBL(uDAL);
                    users[uBL.Email] = uBL;
                    this.aFacade.addToFacade(uBL.Email);
                    Log.Info($"Loaded user into memory: {uBL.Email}");
                }

                Log.Info("LoadAllUsers completed successfully.-USER_FACADE");
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while loading users from database. -USER_FACADE");
                throw;
            }
        }


        /// <summary>
        /// Registers a new user by creating a UserBL object, adding it to the local user dictionary,
        /// and persisting it through the appropriate data access layer.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <exception cref="ArgumentException">Thrown if the email already exists.</exception>
        /// <exception cref="Exception">Propagates any exceptions thrown during user creation.</exception>
        public void Register(string email, string password) 
        {
            Log.Info($"Attempting to register user with email: {email} -USER_FACADE");

            if (this.users.ContainsKey(email))
            {
                Log.Warn($"Registration failed: Email '{email}' already exists. -USER_FACADE");
                throw new ArgumentException("Email already exists");
            }

            UserBL user = null;

            try
            {
                user = new UserBL(email, password);
                Log.Info($"User object created successfully for email: {email} -USER_FACADE");
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to create UserBL object for email '{email}': {ex.Message} -USER_FACADE");
                throw; 
            }

            this.users.Add(email, user);
            this.aFacade.Register(email);
            Log.Info($"User with email '{email}' successfully registered and added to dictionary. -USER_FACADE");
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
            Log.Info($"Attempting login for email: {email} -USER_FACADE");

            if (!this.users.ContainsKey(email))
            {
                Log.Warn($"Login failed: Email '{email}' not found.-USER_FACADE");
                throw new ArgumentException("Email or password incorrect.");
            }

            if (!users[email].Login(password))
            {
                Log.Warn($"Login failed: Incorrect password for email '{email}'. -USER_FACADE");
                throw new ArgumentException("Email or password incorrect.");
            }

            Log.Info($"Login successful for email: {email}  -USER_FACADE");
            aFacade.Login(email);
            return users[email];
        }


        /// <summary>
        /// Logs out the user associated with the given email by delegating to the AuthenticationFacade.
        /// </summary>
        /// <param name="email"></param>
        public void Logout(string email)
        {
            if (!this.aFacade.IsLoggedIn(email))
            {
                Log.Warn("Cant log out while not logged in. - USERFACADE");
                throw new Exception("Cant log out while not logged in");
            }
            Log.Info($"Attempting to log out user with email: {email} -USER_FACADE");
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
    }
}