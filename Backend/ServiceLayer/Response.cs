using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Response<T>
    {
        public string errorMsg;
        public T returnValue;


        public Response() { }

        public Response(string errorMsg, T returnValue)
        {
            this.errorMsg = errorMsg;
            this.returnValue = returnValue;
        }

        public string ErrorMsg
        {
            get { return this.errorMsg; }
            set { this.errorMsg = value; }
        }

        public T ReturnValue
        {
            get { return this.returnValue; }
            set { this.returnValue = value; }
        }
    }
}
