using Logic.Contracts;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.infra
{
    public class ResponseModelMiddlewareHandler
    {
        private readonly RequestDelegate _next;

        public ResponseModelMiddlewareHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            
            context.Response.OnCompleted(()=>{
                if (!context.Response.Body.CanRead)
                    return Task.CompletedTask;

                byte[] bodyBuffer;
                using (var body = context.Response.Body)
                {
                    var numOfBytes = body.Length;
                    bodyBuffer = new byte[numOfBytes];
                    body.Read(bodyBuffer, 0, (int)numOfBytes);
                }
                var bodyStr = Encoding.UTF8.GetString(bodyBuffer);
                var responseModel = JsonConvert.DeserializeObject<ApiResponse>(bodyStr);

                if(responseModel.IsSuccess)
                {
                    _next(context);
                }
                return Task.CompletedTask;
            });
            
            await _next(context);
        }
    }
}
