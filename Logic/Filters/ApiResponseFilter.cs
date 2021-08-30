using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Filters
{
    public class ApiResponseFilter : IOrderedFilter, IResourceFilter
    {
        public int Order { get; } = int.MinValue;
        public void OnResourceExecuting(ResourceExecutingContext context) {
            var data = context;

        }
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            var data = context;
        }
        
    }
}
