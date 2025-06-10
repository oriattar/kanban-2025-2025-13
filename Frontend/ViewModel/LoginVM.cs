using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend.Controllers;
using Frontend.Model;

namespace Frontend.ViewModel
{
    public class LoginVM : Notifiable
    {
        UserController userController = ControllerFactory.Instance.UserController;

        private string email;
        private string password;

        public string Email
        {
            get => email;
            set { email = value; }
        }

        public string Password
        {
            get => password;
            set { password = value; }
        }


        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                RaisePropertyChanged("ErrorMessage");
            }
        }

        internal LoginVM()
        {
            email = string.Empty;
            password = string.Empty;
            errorMessage = "";
        }

        internal UserModel? Login()
        {
            return null;
            
        }


        public UserModel? Register()
        {
            if (string.IsNullOrEmpty(Email))
            {
                ErrorMessage = "email cannot be empty";
                return null;
            }
            if (string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "password cannot be empty";
                return null;
            }
            try
            {
                return userController.Register(Email, Password);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return null;
            }
        }
    }

}
