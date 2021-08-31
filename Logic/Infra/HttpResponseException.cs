using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Logic.Infra
{
    public class HttpResponseException : Exception
    {

        public HttpResponseException(HttpStatusCode statusCode, string description, object value = null)
        {
            StatusCode = statusCode;
            Description = description;
            Value = value;
        }
        public HttpStatusCode StatusCode { get;} = HttpStatusCode.InternalServerError;
        public string Description { get; set; }
        public object Value { get; set; }
    }
}
