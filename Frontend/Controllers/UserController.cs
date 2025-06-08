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
            var json = Us.Login(username, password);
            var response = JsonSerializer.Deserialize<Response<string>>(json);

            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);

            return new UserModel(response.ReturnValue, this);
        }

        public UserModel Register(string username, string password)
        {
            var json = Us.Register(username, password);
            var response = JsonSerializer.Deserialize<Response<string>>(json);

            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);

            return new UserModel(response.ReturnValue, this);
        }

    }

}
