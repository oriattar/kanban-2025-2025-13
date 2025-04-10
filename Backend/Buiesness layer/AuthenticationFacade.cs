using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.Buiesness_layer
{
    class AuthenticationFacade
    {
        private Dictionary<string, bool> uStatus;

        public AuthenticationFacade(Dictionary<string, bool> uStatus)
        {
            this.uStatus = uStatus;
        }

        
        public Dictionary<string, bool> UStatus { get => uStatus; set => uStatus = value; }

        public bool IsLoggedIn(string un)
        {
            throw new NotImplementedException();
        }

        public bool Login(string un)
        {
            throw new NotImplementedException();
        }
    }
}
