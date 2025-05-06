using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("UnitTests")]
[assembly: InternalsVisibleTo("BackendTests")]


namespace IntroSE.Kanban.Backend.ServiceLayer
{
    internal class Response<T>
    {
        public string ErrorMessage { get; set; }
        public T? ReturnValue { get; set; }


        public Response() { }

        /// <summary>
        /// A constructor for respone, with no return value input
        /// </summary>
        /// <param name="errorMsg"></param>
        public Response(string errorMsg)
        {
            this.ErrorMessage = errorMsg;
        }

        /// <summary>
        /// Creates a response with both an error message and return value.
        /// </summary>
        /// <param name="errorMsg">The error message.</param>
        /// <param name="returnValue">The return value.</param>
        public Response(string errorMsg, T returnValue)
        {
            this.ErrorMessage = errorMsg;
            this.ReturnValue = returnValue;
        }

    }
}

