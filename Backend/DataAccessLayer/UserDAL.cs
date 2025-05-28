using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buiesness_layer;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class UserDAL
    {
        public const string UserEmail = "Email";
        public const string UserPassword = "Password";

        private string email;
        private string password;
        bool isPersisted = false;

        private readonly UserController controller;

        public UserDAL(string email, string password)
        {
            this.email = email;
            this.password = password;
            this.controller = new UserController();
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
            }
        }

        public UserController Controller { get; set; }

       

    }
}
