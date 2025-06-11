using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend.Controllers;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Frontend.Model
{
    public class UserModel
    {


        public string Email { get; }
        

        private bool isNewUser;

        internal UserModel(string email)
        {
            this.Email = email;
        }

    }
}
