using EntityServer.Contracts;
using Logic.Infra;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

            }
            catch(HttpResponseException ex)
            {
                context.Response.ContentType = "text/json";
                await context.Response.WriteAsync(new ApiResponse(false,
                                                                    Convert.ToInt32(ex.StatusCode),
                                                                    ex.Description)
                                                                    .ToString());
            }
            catch(Exception ex)
            {
                await HandleException(context,ex.Message);
            }
        }

        private async Task HandleException(HttpContext context, string errMsg)
        {
            context.Response.ContentType = "text/json";
            await context.Response.WriteAsync(  new ApiResponse(false, 
                                                                Convert.ToInt32(HttpStatusCode.InternalServerError),
                                                                errMsg)
                                                                .ToString());
        }
    }
}
