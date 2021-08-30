using EntityServer.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EntityServer.Infra.Extensions
{
    public static class ExceptionsMiddlewareExtension
    {
        public static void ConfigureExceptionsMiddleware(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError => {
                appError.Run(async context => {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "text/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(contextFeature != null)
                    {
                        //logger.LogError("something went wrong:" + contextFeature.Error);
                        await context.Response.WriteAsync( new ApiErrorResponse(context.Response.StatusCode,
                                                                                "something really went wrong")
                                                                                .ToString());
                    }
                });
            });
        }
        public static void ConfigureCustomExceptionsMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionsCustomMiddlewareExtension>();
        }

        public static void ConfigureTestExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionCustomTestMiddelware>();
        }
    }
}
