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

        public HttpResponseException(HttpStatusCode statusCode, string description)
        {
            StatusCode = statusCode;
            Description = description;
        }
        public HttpStatusCode StatusCode = HttpStatusCode.InternalServerError;
        public string Description { get; set; }
        public object Value { get; set; }
    }
}
