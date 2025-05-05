using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buiesness_layer;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    internal class ServiceFactory
    {
        public TaskServices ts;
        public BoardServices bs;
        public UserServices us;

        /// <summary>
        /// Inits the services objects, the SF consturcotr
        /// </summary>
        public ServiceFactory()
        {
            AuthenticationFacade af = new AuthenticationFacade();
            UserFacade uf = new UserFacade(af);
            BoardFacade bf = new BoardFacade(af); //inits the facades


            this.us = new UserServices(uf);
            this.bs = new BoardServices(uf, bf);
            this.ts = new TaskServices(uf, bf); //creates the services
        }


    }
}
