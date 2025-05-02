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

        /// <summary>
        /// A constructor for respone, with no return value input
        /// </summary>
        /// <param name="errorMsg"></param>
        public Response(string errorMsg)
        {
            this.errorMsg = errorMsg;
        }
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
