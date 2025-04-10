using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buiesness_layer;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class ServiceFactory
    {
        public TaskServices ts;
        public BoardServices bs;
        public UserServices us;

        
        public ServiceFactory()
        {
            throw new NotImplementedException();
        }

        
    }
}
