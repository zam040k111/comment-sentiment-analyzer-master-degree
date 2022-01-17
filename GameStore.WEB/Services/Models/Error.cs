using GameStore.WEB.Models;

namespace GameStore.WEB.Services.Models
{
    public static class Error
    {
        public static ErrorDetails _500 = new ErrorDetails {Message = "Server error.", StatusCode = 500};
        public static ErrorDetails _404 = new ErrorDetails {Message = "The object was not found.", StatusCode = 404};
    }
}
