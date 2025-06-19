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
using IntroSE.Kanban.Backend.DataAccessLayer;
using System.Text.Json;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class ServiceFactory
    {
        public TaskServices ts;
        public BoardServices bs;
        public UserServices us;

        private SystemController systemController;

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public UserServices Us { get; set; }

        /// <summary>
        /// Inits the services objects, the SF consturcotr
        /// </summary>
        public ServiceFactory()
        {
            this.systemController = new SystemController();
            AuthenticationFacade af = new AuthenticationFacade();
            UserFacade uf = new UserFacade(af);
            BoardFacade bf = new BoardFacade(af,systemController); //inits the facades

            var logRepo = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepo, new FileInfo("log4net.config")); //inits the logger


            this.us = new UserServices(uf);
            this.bs = new BoardServices(bf);
            this.ts = new TaskServices(bf); //creates the services

        }

        /// <summary>
        /// Loads the data on startup service
        /// </summary>
        /// <returns></returns>
        public string LoadData()
        {
            Response<string> res = null;
            try
            {
                this.us.LoadUsers(); //loads user data
                this.bs.LoadBoards();//loads board data
                res = new Response<string>(null, null);
            }
            catch (Exception ex)
            {
                res = new Response<string>(ex.Message, null);
            }
            return JsonSerializer.Serialize(res);
        }
        /// <summary>
        /// Clears data from db using system controller
        /// </summary>
        public string ClearData()
        {
            Response<string> res = null;
            try
            {
                this.systemController.ClearData();
                res = new Response<string>(null, null);
            }
            catch (Exception ex)
            {
                res = new Response<string>(ex.Message, null);
            }
            return JsonSerializer.Serialize(res);
        }


    }
}
