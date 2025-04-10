using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.Buiesness_layer
{
    class UserFacade
    {
        private Dictionary<string, UserBL> users;
        private AuthenticationFacade aFacade;

        public UserFacade(Dictionary<string, UserBL> users, AuthenticationFacade aFacade)
        {
            this.users = users;
            this.aFacade = aFacade;
        }

        public Dictionary<string, UserBL> Users { get => users; set => users = value; }
        public AuthenticationFacade AFacade { get => aFacade; set => aFacade = value; }

        public UserBL Login(string email, string password)
        {
            throw new NotImplementedException();
        }

        public UserBL Register(string email, string password) 
        {
            throw new NotImplementedException();
        }

        public UserBL GetUserBL(string email)
        {
            throw new NotImplementedException();
        }

        public BoardBL CreateBoard(string name, String email)
        {
            throw new NotImplementedException();
        }

    }
}
