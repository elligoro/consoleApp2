using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Logic.Infra
{
    // https://www.infoworld.com/article/2994111/how-to-handle-errors-in-aspnet-web-api.html
    public class WebApiException : HttpResponseException
    {
        private readonly HttpStatusCode _status;
        private readonly ApiErrorsEnum _errorType;
        public WebApiException(HttpResponseMessage response, HttpStatusCode status,ApiErrorsEnum errorType ):base(response)
        {
            _status = status;
            _errorType = errorType;
        }

        public WebApiException(HttpResponseMessage response, HttpStatusCode status) : base(response)
        {
            _status = status;
        }

        public ApiErrorsEnum ErrorType 
        { get
            {
                return _errorType;
            }
        }

        public HttpStatusCode Status
        {
            get
            {
                return _status;
            }
        }

    }
}
