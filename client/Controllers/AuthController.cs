using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using client.Business;
using Logic.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<string> TryAuthenticate()
        {
            return await _authLogic.TryAuthenticate(HttpContext.Request, HttpContext.Response);
        }
    }
}
