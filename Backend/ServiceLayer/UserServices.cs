using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buiesness_layer;
using log4net;


namespace IntroSE.Kanban.Backend.ServiceLayer
{
    internal class UserServices
    {
        private UserFacade uFacade;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Constructor for the userFacade
        /// </summary>
        /// <param name="uFacade"></param>
        public UserServices(UserFacade uFacade)
        {
            this.uFacade = uFacade;
        }

        public UserFacade UFacade { get => uFacade; set => uFacade = value; }
        /// <summary>
        /// Logs in a user with the provided email and password.
        /// </summary>
        /// <param name="email">The user's email address</param>
        /// <param name="password">The user's password</param>
        /// <returns>A Response containing either the logged-in user or an error message</returns>
        public Response<UserSL> Login(string email, string password)
        {
            try
            {
                Log.Info($"Attempting to log in user with email: {email}");
                UserBL user = uFacade.Login(email, password);
                Log.Info($"User with email {email} logged in successfully.");
                return new Response<UserSL>(null, new UserSL(user));
            }
            catch (Exception ex)
            {
                Log.Error($"Login failed for user {email}: {ex.Message}");
                return new Response<UserSL>(ex.Message, null);
            }
        }


        /// <summary>
        /// Logs out a user with the given email.
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>A Response containing either a success message or an error message</returns>
        public Response<string> Logout(string email)
        {
            try
            {
                Log.Info($"Attempting to log out user with email: {email}");
                uFacade.Logout(email);
                Log.Info($"User with email {email} logged out successfully.");
                return new Response<string>(null, "Logged out successfully");
            }
            catch (Exception ex)
            {
                Log.Error($"Logout failed for user {email}: {ex.Message}");
                return new Response<string>(ex.Message, null);
            }
        }

        /// <summary>
        /// Registers a new user with the provided email and password.
        /// </summary>
        /// <param name="email">The email of the new user</param>
        /// <param name="password">The password of the new user</param>
        /// <returns>A Response containing either the registered user or an error message</returns>
        public Response<UserSL> Register(string email, string password)
        {
            try
            {
                Log.Info($"Attempting to register user with email: {email}");
                UserBL user = uFacade.Register(email, password);
                Log.Info($"User with email {email} registered successfully.");
                return new Response<UserSL>(null, new UserSL(user));
            }
            catch (Exception ex)
            {
                Log.Error($"Registration failed for user {email}: {ex.Message}");
                return new Response<UserSL>(ex.Message, null);
            }
        }
    }
}
