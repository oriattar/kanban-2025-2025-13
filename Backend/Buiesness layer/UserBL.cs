using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;
using log4net;

[assembly: InternalsVisibleTo("UnitTests")]

namespace IntroSE.Kanban.Backend.Buiesness_layer
{

    internal class UserBL
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string email;
        private string password;
        internal List<BoardBL> boards;



        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                Log.Info($"Attempting to set email: {value}");

                if (string.IsNullOrWhiteSpace(value) ||!value.Contains('@') || !value.Contains('.') ||value.Contains(" "))
                {
                    Log.Warn("Invalid email format.");
                    throw new ArgumentException("Email cannot be like that.");
                }

                int atIndex = value.IndexOf('@');
                int lastDotIndex = value.LastIndexOf('.');

                if (atIndex <= 0 || atIndex == value.Length - 1 ||
                    lastDotIndex < atIndex + 2 || lastDotIndex == value.Length - 1)
                {
                    Log.Warn("Email format failed domain check.");
                    throw new ArgumentException("Email cannot be like that.");
                }

                email = value;
                Log.Info($"Email set successfully: {email}");
            }
        }

        public string Password { private get => password;
            set
            {
                Log.Info("Attempting to set password.");
                if (string.IsNullOrWhiteSpace(value))
                {
                    Log.Warn("Password validation failed: password is null, empty, or whitespace.");
                    throw new ArgumentException("Password cannot be empty.");
                }
                if (value.Length < 6)
                {
                    Log.Warn($"Password validation failed: password too short (length {value.Length}).");
                    throw new ArgumentException("Password must be between 6 and 20 characters.");
                }

                if (value.Length > 20)
                {
                    Log.Warn($"Password validation failed: password too long (length {value.Length}).");
                    throw new ArgumentException("Password must be between 6 and 20 characters.");
                }

                if (!value.Any(char.IsUpper))
                {
                    Log.Warn("Password validation failed: no uppercase letter found.");
                    throw new ArgumentException("Password must contain at least one uppercase letter.");
                }

                if (!value.Any(char.IsLower))
                {
                    Log.Warn("Password validation failed: no lowercase letter found.");
                    throw new ArgumentException("Password must contain at least one lowercase letter.");
                }

                if (!value.Any(char.IsDigit))
                {
                    Log.Warn("Password validation failed: no digit found.");
                    throw new ArgumentException("Password must contain at least one digit.");
                }

                password = value;
                Log.Info("Password was successfully set.");
            }
        }

        public List<BoardBL> Boards { get => boards; set => boards = value; }
       
        /// <summary>
        /// UserBL object constructor
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public UserBL(string email, string password)
        {
            Log.Info("Creating new UserBL object.");
            Email = email;
            Password = password;
            this.boards = new List<BoardBL>();
            Log.Info($"User created successfully: {email}");

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
                Log.Info($"Login successful for user '{email}'");
            else
                Log.Warn("Login failed - incorrect password.");

            return success;
        }
    }
}
