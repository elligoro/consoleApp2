using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EntityServer.Contracts
{
    public class ApiResponse
    {
        public ApiResponse(bool isSuccess, int statusCode, string dataModel)
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            DataModel = dataModel;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string DataModel { get; set; }
    }
}
