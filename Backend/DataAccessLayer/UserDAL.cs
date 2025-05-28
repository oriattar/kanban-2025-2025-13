using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buiesness_layer;
using log4net;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class UserDAL
    {
        public const string UserEmail = "Email";
        public const string UserPassword = "Password";

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        private string email;
        private string password;
        bool isPersisted ;

        private readonly UserController controller;


        /// <summary>
        /// Constructor for UserDAL with email and password.
        /// </summary>
        public UserDAL(string email, string password)
        {
            this.email = email;
            this.password = password;
            this.controller = new UserController();
            isPersisted = false;

        }


        /// <summary>
        /// Gets or sets the user's email.
        /// </summary>
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
            }
        }


        /// <summary>
        /// Gets or sets the UserController instance associated with this user.
        /// </summary>

        public UserController Controller { get; }


        /// <summary>
        /// Saves the user data to the database if it has not already been persisted.
        /// Throws an exception if the object is already persisted.
        /// </summary>
        public void Save()
        {
            Log.Info($" Attempting to save user: {Email} - USERDAL");

            if (isPersisted)
            {
                Log.Warn($" Save failed: User '{Email}' is already persisted. - USERDAL");
                throw new InvalidOperationException("Cannot save a persisted object. ");
            }

            
             controller.Insert(this);
             isPersisted = true;
             Log.Info($" User '{Email}' saved successfully. - USERDAL");
            
            
        }

    }
}
