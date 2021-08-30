using EntityServer.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityServer.Infra.Extensions
{
    public class ExceptionCustomTestMiddelware
    {
        private readonly RequestDelegate _next;

        public ExceptionCustomTestMiddelware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }catch(Exception ex)
            {
                await HandleException(context);
            }
        }

        private async Task HandleException(HttpContext context)
        {
            context.Response.ContentType = "text/json";
            await context.Response.WriteAsync(  new ApiResponse(false, 
                                                                context.Response.StatusCode, 
                                                                "OOPS!")
                                                                .ToString());
                                                        }
    }
}
