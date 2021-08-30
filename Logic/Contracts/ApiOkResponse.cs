using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Contracts
{
    public class ApiOkResponse
    {
        public object Result { get; }

        public ApiOkResponse(object result)
        {
            Result = result;
        }
    }
}
