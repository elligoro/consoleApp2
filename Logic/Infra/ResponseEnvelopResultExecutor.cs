using Logic.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Infra
{
    public class ResponseEnvelopResoooultExecutor : ObjectResultExecutor
    {
        public ResponseEnvelopResoooultExecutor(OutputFormatterSelector formatterSelector, 
                                             IHttpResponseStreamWriterFactory writeFactory, 
                                             ILoggerFactory loggerFactory) : base(formatterSelector, writeFactory, loggerFactory)                                           
        {
        }

        public override Task ExecuteAsync(ActionContext context, ObjectResult result)
        {
            result.Value = JsonConvert.DeserializeObject<ApiResponse>(result.Value.ToString());

            return base.ExecuteAsync(context, result);
        }

    }
}
