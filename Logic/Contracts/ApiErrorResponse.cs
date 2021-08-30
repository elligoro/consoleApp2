using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Contracts
{
    public class ApiErrorResponse
    {
        public string ErrorMsg { get; }
        public ApiErrorResponse(int? statusCode, string errorMsg=null)
        {
            ErrorMsg = errorMsg;
        }
    }
}
