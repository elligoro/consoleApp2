using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web.Http.Filters;
using System.Net.Http;

namespace Logic.Infra
{
    // https://www.infoworld.com/article/2994111/how-to-handle-errors-in-aspnet-web-api.html
    public class WebApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;
            var message = string.Empty;

            var exceptionType = actionExecutedContext.Exception.GetType();

            if(exceptionType == typeof(UnauthorizedAccessException))
            {
                message = "Access is not authorized";
                status = HttpStatusCode.Unauthorized;
            }
            else
            {
                message = "general error";
            }

            actionExecutedContext.Response = new HttpResponseMessage
            {
                Content = new StringContent(message, Encoding.UTF8,"text/plain"),
                StatusCode = status
            };

            base.OnException(actionExecutedContext);
        }
    }
}
