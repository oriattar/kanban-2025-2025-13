using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buiesness_layer;


namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class UserServices
    {
        private UserFacade uFacade;

        /// <summary>
        /// Constructor for the userFacade
        /// </summary>
        /// <param name="uFacade"></param>
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
