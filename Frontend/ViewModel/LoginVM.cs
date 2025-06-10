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

            
        }


        public UserModel? Register()
        {
            
        }
    }

}
