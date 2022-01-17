using System;
using System.Threading.Tasks;
using GameStore.BLL.Exceptions.ServiceExceptions;
using GameStore.BLL.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GameStore.WEB.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly INLog _logger;

        public ExceptionMiddleware(RequestDelegate next, INLog logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (HttpException ex)
            {
                _logger.Warning($"Something went wrong: {ex}");
                httpContext.Response.StatusCode = (int) ex.HttpStatusCode;
            }
            catch (Exception ex)
            {
                _logger.Error($"Something went wrong: {ex}");
                httpContext.Response.StatusCode = 500;
            }
        }
    }
}
