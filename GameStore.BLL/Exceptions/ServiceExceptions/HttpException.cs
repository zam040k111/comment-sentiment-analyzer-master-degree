using System;
using System.Net;

namespace GameStore.BLL.Exceptions.ServiceExceptions
{
    public class HttpException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; }

        public HttpException(string message) : base(message)
        {
            HttpStatusCode = HttpStatusCode.BadRequest;
        }

        public HttpException(string message, HttpStatusCode statusCode) : base(message)
        {
            HttpStatusCode = statusCode;
        }
    }
}
