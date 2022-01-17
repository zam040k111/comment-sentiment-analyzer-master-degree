using System.Net;

namespace GameStore.BLL.Exceptions.ServiceExceptions
{
    public class NotFoundException : HttpException
    {
        public NotFoundException() : base("The object was not found", HttpStatusCode.NotFound) { }
    }
}
