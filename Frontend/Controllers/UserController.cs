using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Frontend.Model;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Frontend.Controllers
{
    public class UserController
    {
        private UserServices Us;

        public UserController(UserServices Us)
        {
            this.Us = Us;
        }



        public UserModel Login(string username, string password)
        {
            return null;
        }

        public UserModel Register(string username, string password)
        {
            return null;

        }

    }

}
