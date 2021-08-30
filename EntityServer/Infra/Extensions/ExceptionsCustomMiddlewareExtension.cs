using EntityServer.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityServer.Infra.Extensions
{
    public class ExceptionsCustomMiddlewareExtension
    {
        private readonly RequestDelegate _next;

        public ExceptionsCustomMiddlewareExtension(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(httpContext);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "text/json";

            await httpContext.Response.WriteAsync(new ApiErrorResponse(httpContext.Response.StatusCode, 
                                                                       "error happened")
                                                                        .ToString());
        }
    }
}
