using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Frontend.Controllers
{
    public class ControllerFactory
    {
        public static ControllerFactory Instance { get; } = new ControllerFactory();
        public UserController UserController { get; internal set; }

        public readonly UserController userController;

        private ControllerFactory()
        {
            ServiceFactory serviceFactory = new ServiceFactory();
            userController = new UserController(serviceFactory.Us);
        }
    }

}
