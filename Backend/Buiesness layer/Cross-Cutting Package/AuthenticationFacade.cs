using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using log4net;




namespace IntroSE.Kanban.Backend.Buiesness_layer
{
    public class AuthenticationFacade
    {
        private Dictionary<string, bool> uStatus;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// A constructor for the facade, inits the dictionary
        /// </summary>
        public AuthenticationFacade()
        {
            this.uStatus = new Dictionary<string, bool>();
        }

        /// <summary>
        /// returns the status of the input user in the dictionary
        /// true if logged in, else false.
        /// </summary>
        /// <param name="un"></param>
        /// <returns></returns>
        public bool IsLoggedIn(string un)
        {
            if (!this.uStatus.ContainsKey(un))
            {
                Log.Warn("Checking log in for unregisted user -AUTH_FACADE");
                throw new Exception("User is not registerd in authenticationFacade -AUTH_FACADE");
            }
                

            return this.uStatus[un];
        }



        /// <summary>
        /// Logging in a registerd user by updating its value in the dict, if user isnt registerd throws exception
        /// </summary>
        /// <param name="un"></param>
        /// <exception cref="Exception"></exception>
        public void Login(string un)
        {
            if (!this.uStatus.ContainsKey(un))
            {
                Log.Warn("Trying to log in an unregisterd user -AUTH_FACADE");
                throw new Exception("Trying to log in an unregisterd user in authentication facade -AUTH_FACADE");
            }

            this.uStatus[un] = true;
                
        }
        /// <summary>
        /// Registers a user in the dictionary, and logs it in
        /// </summary>
        /// <param name="un"></param>
        public void Register(string un)
        {
            this.uStatus.Add(un, true);  //automaticaly logs in
        }
        /// <summary>
        /// Logging out a user by updating in the dictionary its value to false.
        /// Throws exception if user isnt registerd in the dict.
        /// </summary>
        /// <param name="un"></param>
        /// <exception cref="Exception"></exception>
        public void Logout(string un)
        {
            if (!this.uStatus.ContainsKey(un))
            {
                Log.Warn("Trying to log in an unregisterd user -AUTH_FACADE");
                throw new Exception("Cant log out unregisterd user from authentication facade");
            }

            this.uStatus[un] = false;
        }

        /// <summary>
        /// Using to register users while loading the system
        /// not logging the user automatically
        /// </summary>
        /// <param name="un"></param>
        public void addToFacade(string un)
        {
            this.uStatus[un] = false;
        }

    }
}
