using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class UserServices
    {
        private UserFacade uFacade;

        public UserServices(UserFacade uFacade)
        {
            this.uFacade = uFacade;
        }

        public UserFacade UFacade { get => uFacade; set => uFacade = value; }

        public Response<UserSL> Login(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Response<string> Logout(string email)
        {
            throw new NotImplementedException();
        }

        public Response<UserSL> Register(string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
