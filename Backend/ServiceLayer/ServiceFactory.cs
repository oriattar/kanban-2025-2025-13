using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buiesness_layer;
using log4net.Config;
using log4net;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    internal class ServiceFactory
    {
        public TaskServices ts;
        public BoardServices bs;
        public UserServices us;

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Inits the services objects, the SF consturcotr
        /// </summary>
        public ServiceFactory()
        {
            AuthenticationFacade af = new AuthenticationFacade();
            UserFacade uf = new UserFacade(af);
            BoardFacade bf = new BoardFacade(af); //inits the facades

            var logRepo = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepo, new FileInfo("log4net.config")); //inits the logger


            this.us = new UserServices(uf);
            this.bs = new BoardServices(bf);
            this.ts = new TaskServices(bf); //creates the services

            
        }


    }
}
