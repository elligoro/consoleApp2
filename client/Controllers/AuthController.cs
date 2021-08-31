using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using client.Business;
using Logic.Contracts;
using Logic.Filters;
using Logic.Infra;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace client.Controllers
{
    [ServiceFilter(typeof(ApiResponseFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthLogic _authLogic;
        public AuthController(AuthLogic authLogic)
        {
            _authLogic = authLogic;
        }

        [Route("login")]
        [HttpPost]
        public async Task<ApiResponse> TryAuthenticate()
        {
            return await _authLogic.TryAuthenticate(HttpContext.Request, HttpContext.Response);
        }
    }
}
