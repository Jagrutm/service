using BuildingBlocks.Core.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Common.Logging.Middleware
{
    public sealed class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<ErrorLoggingMiddleware> _logger;

        public ErrorLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context, ILogger<ErrorLoggingMiddleware> logger)
        {
            try
            {
                _logger = logger;
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: ex.Message);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(new ErrorDetails
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = ex.Message
                }.ToString());
            }
        }
    }

    public static class ErrorLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorLoggingMiddleware(this IApplicationBuilder builder)
            => builder.UseMiddleware<ErrorLoggingMiddleware>();
    }
}
