using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using log4net;

[assembly: InternalsVisibleTo("UnitTests")]

namespace IntroSE.Kanban.Backend.Buiesness_layer
{

    internal class UserBL
    {
        private static readonly int PASSWORD_MIN_LENGTH = 6;
        private static readonly int PASSWORD_MAX_LENGTH = 20;
        
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string email;
        private string password;

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                Log.Info($"Attempting to set email: {value} -USER_BL");

                if (string.IsNullOrWhiteSpace(value) ||!value.Contains('@') || !value.Contains('.') ||value.Contains(" "))
                {
                    Log.Warn("Invalid email format.-USER_BL");
                    throw new ArgumentException("Email cannot be like that.");
                }

                int atIndex = value.IndexOf('@');
                int lastDotIndex = value.LastIndexOf('.');

                if (atIndex <= 0 || atIndex == value.Length - 1 ||
                    lastDotIndex < atIndex + 2 || lastDotIndex == value.Length - 1)
                {
                    Log.Warn("Email format failed domain check. -USER_BL");
                    throw new ArgumentException("Email cannot be like that.");
                }
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                if (!regex.IsMatch(value))
                {
                    Log.Warn("Email format failed domain check. -USER_BL");
                    throw new ArgumentException("Email cannot be like that.");
                }


                email = value;
                Log.Info($"Email set successfully: {email} -USER_BL");
            }
        }

        public string Password { private get => password;
            set
            {
                Log.Info("Attempting to set password. -USER_BL");
                if (string.IsNullOrWhiteSpace(value))
                {
                    Log.Warn("Password validation failed: password is null, empty, or whitespace. -USER_BL");
                    throw new ArgumentException("Password cannot be empty.");
                }
                if (value.Length < PASSWORD_MIN_LENGTH)
                {
                    Log.Warn($"Password validation failed: password too short (length {value.Length}). -USER_BL");
                    throw new ArgumentException("Password must be between 6 and 20 characters.");
                }

                if (value.Length > PASSWORD_MAX_LENGTH)
                {
                    Log.Warn($"Password validation failed: password too long (length {value.Length}). -USER_BL");
                    throw new ArgumentException("Password must be between 6 and 20 characters.");
                }

                if (!value.Any(char.IsUpper))
                {
                    Log.Warn("Password validation failed: no uppercase letter found. -USER_BL");
                    throw new ArgumentException("Password must contain at least one uppercase letter.");
                }

                if (!value.Any(char.IsLower))
                {
                    Log.Warn("Password validation failed: no lowercase letter found. -USER_BL");
                    throw new ArgumentException("Password must contain at least one lowercase letter.");
                }

                if (!value.Any(char.IsDigit))
                {
                    Log.Warn("Password validation failed: no digit found. -USER_BL");
                    throw new ArgumentException("Password must contain at least one digit.");
                }

                password = value;
                Log.Info("Password was successfully set. -USER_BL");
            }
        }

        
       
        /// <summary>
        /// UserBL object constructor
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public UserBL(string email, string password)
        {

            Log.Info($" Starting creation of UserBL for email: {email}  -USER_BL");

            try
            {
                UserDAL dto = new UserDAL(email, password);
                Log.Info($" UserDAL created for email: {email}  -USER_BL");

                Email = email;
                Password = password;

               
            }
            catch (Exception ex)
            {
                Log.Error($" Error during creation of UserBL for email: {email}: {ex.Message}  -USER_BL", ex);
                throw;
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="UserBL"/> class from an existing <see cref="UserDAL"/> object.
        /// </summary>
        /// <param name="user">The UserDAL object containing the user's email and password.</param>
        public UserBL(UserDAL user)
        {
            Log.Info("Creating new UserBL object. -USER_BL");
            Email = user.Email;
            Password = user.Password;

            Log.Info($"User created successfully: {email} -USER_BL");

        }

        /// <summary>
        /// check if the password is correct and login
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Login (string password)
        {
            bool success = this.password == password;

            if (success)
                Log.Info($"Login successful for user '{email}' -USER_BL");
            else
                Log.Warn("Login failed - incorrect password. -USER_BL");

            return success;
        }
    }
}
