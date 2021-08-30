using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace client.infra
{
    public static class MiddlewareExtensionMethods
    {
        public static void ConfigureResponseModelMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ResponseModelMiddlewareHandler>();
        }
    }
}
